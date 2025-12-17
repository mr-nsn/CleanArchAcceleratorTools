using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Types;
using CleanArchAcceleratorTools.Domain.Util;
using System.Linq.Expressions;
using System.Reflection;

namespace CleanArchAcceleratorTools.Infrastructure.Select.Helpers;

/// <summary>
/// Utilities to build dynamic LINQ select expressions from field names (supports nested paths and collections).
/// </summary>
/// <remarks>
/// - Supports dot notation (e.g., "Address.City") and constructs nested initializers.
/// - For collections, projects selected item fields and materializes to lists.
/// - Only requested properties are populated; others keep defaults.
/// Requirements:
/// - Target types (and collection item types) need public parameterless constructors.
/// - Selected properties must be writable and not marked as NotMapped when used with EF.
/// </remarks>
internal static class SelectHelper
{
    /// <summary>
    /// Pool of short parameter names for nested lambdas.
    /// </summary>
    private static List<string> _avaibleParameters = Enumerable.Empty<string>().ToList();

    /// <summary>
    /// Generates a projection that initializes a new <typeparamref name="T"/> with only requested fields.
    /// </summary>
    /// <typeparam name="T">Entity type to project.</typeparam>
    /// <param name="fields">Field names (dot notation supported).</param>
    /// <returns>Expression of type Expression&lt;Func&lt;T, T&gt;&gt;.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fields"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when a property path is invalid.</exception>
    public static Expression<Func<T, T>> DynamicSelectGenerator<T>(string[] fields)
    {
        if (fields is null || !fields.Any())
            throw new ArgumentNullException(nameof(fields), DomainMessages.PropertyCannotBeEmpty.ToFormat(nameof(fields)));

        InitializeAvaiableParameters();

        // Group the fields by their first part (before any punctuation) to handle nested properties
        var groupedEntityFields = fields
            .GroupBy(g => string.Join("", g.TakeWhile(s => !char.IsPunctuation(s))))
            .ToDictionary(g => g.Key, g => g.Select(s => s.Contains(".") ? s.Substring(g.Key.Length + 1) : s).ToList());

        // Input parameter "o"
        var xParameter = Expression.Parameter(typeof(T), "o");

        // New statement "new T()"
        var xNew = Expression.New(typeof(T));

        // Create initializers
        var bindings = groupedEntityFields
            .Select(d => CreateBinding(typeof(T), d, xParameter))
            .ToList();

        // Initialization "new T { Field1 = o.Field1, Field2 = o.Field2... }"
        var xInit = Expression.MemberInit(xNew, bindings);

        // Expression "o => new T { Field1 = o.Field1, Field2 = o.Field2 }"
        var lambda = Expression.Lambda<Func<T, T>>(xInit, xParameter);

        // Compile to Func<T, T>
        return lambda;
    }

    /// <summary>
    /// Creates a top-level member binding, delegating nested segments.
    /// </summary>
    /// <param name="type">Root type.</param>
    /// <param name="propertyAndPaths">Key: property name; Value: nested segments.</param>
    /// <param name="xParameter">Root parameter.</param>
    /// <returns>Member assignment for the property.</returns>
    /// <exception cref="ArgumentException">Thrown when the property is not found.</exception>
    private static MemberAssignment CreateBinding(Type type, KeyValuePair<string, List<string>> propertyAndPaths, Expression xParameter)
    {
        var keyProperty = type.GetProperty(propertyAndPaths.Key);
        if (keyProperty == null)
            throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(propertyAndPaths.Key, type.Name));

        return CreateBinding(keyProperty, propertyAndPaths.Value, xParameter);
    }

    /// <summary>
    /// Recursively creates bindings for simple values, complex types, and collections.
    /// </summary>
    /// <param name="keyProperty">Property to bind.</param>
    /// <param name="nestedProperties">Nested segments.</param>
    /// <param name="xParameter">Current parameter/member expression.</param>
    /// <returns>Member assignment binding.</returns>
    /// <exception cref="ArgumentException">Thrown when a nested property is not found.</exception>
    /// <exception cref="Exception">Thrown for unsupported types.</exception>
    private static MemberAssignment CreateBinding(PropertyInfo keyProperty, List<string> nestedProperties, Expression xParameter)
    {
        Type nestedType = keyProperty.PropertyType;
        Expression nestedParam = Expression.Property(xParameter, keyProperty);

        if (nestedProperties.Any(p => p.Contains('.'))) // Recursive case (When a nested property have a punctuation)
        {
            var newNestedProperties = nestedProperties
                .GroupBy(g => g.Contains('.') ? string.Join("", g.TakeWhile(s => !char.IsPunctuation(s))) : keyProperty.Name)
                .ToDictionary(g => g.Key, g => g.Select(s => s.Contains('.') ? s.Substring(g.Key.Length + 1) : s).ToList());

            if (CleanArchTypeExtensions.IsCollection(nestedType))
            {
                var itemType = nestedType.IsGenericType
                    ? nestedType.GetGenericArguments()[0]
                    : nestedType.GetElementType();

                var newNestedParam = Expression.Parameter(itemType!, GetNextParameter());

                var nestedBindings = newNestedProperties.SelectMany(p =>
                {
                    if (p.Key == keyProperty.Name)
                    {
                        return CreateMemberBindingHelper(itemType!, p.Value, newNestedParam);
                    }
                    else
                    {
                        var newKeyProperty = itemType!.GetProperty(p.Key);
                        return new MemberAssignment[] { CreateBinding(newKeyProperty!, p.Value, newNestedParam) };
                    }
                }).ToArray();

                var newItem = Expression.MemberInit(Expression.New(itemType!), nestedBindings);
                var selectLambda = Expression.Lambda(newItem, newNestedParam);

                var newNested = CreateSelectCall(itemType!, selectLambda, nestedParam);
                return Expression.Bind(keyProperty, newNested);
            }
            // Threat when the nested property is an simple object
            else
            {
                var nestedBindings = newNestedProperties.SelectMany(p =>
                {
                    if (p.Key == keyProperty.Name)
                    {
                        return CreateMemberBindingHelper(nestedType, p.Value, nestedParam);
                    }
                    else
                    {
                        var newKeyProperty = nestedType.GetProperty(p.Key);
                        return new MemberAssignment[] { CreateBinding(newKeyProperty!, p.Value, nestedParam) };
                    }
                }).ToArray();

                var newNested = Expression.MemberInit(Expression.New(nestedType), nestedBindings);
                return Expression.Bind(keyProperty, newNested);
            }
        }
        else // Base case (When a nested property does not have a punctuation)
        {
            if (CleanArchTypeExtensions.IsSimple(nestedType)) // Simple property (only create the binding)
            {
                var propertyExpr = Expression.Property(xParameter, keyProperty);
                return Expression.Bind(keyProperty, propertyExpr);
            }
            else if (CleanArchTypeExtensions.IsComplex(nestedType)) // Complex property (create binding for all the properties selected)
            {
                var nestedBindings = CreateMemberBindingHelper(nestedType, nestedProperties, nestedParam);
                var newNested = Expression.MemberInit(Expression.New(nestedType), nestedBindings);
                return Expression.Bind(keyProperty, newNested);
            }
            else if (CleanArchTypeExtensions.IsCollection(nestedType)) // Lists or arrays (create a binding for the list)
            {
                var nestedBindings = CreateListBindingHelper(nestedType, nestedProperties, nestedParam);
                return Expression.Bind(keyProperty, nestedBindings);
            }
            else
            {
                throw new Exception(DomainMessages.UnsupportedTypeForProperty.ToFormat(keyProperty.Name, nestedType.Name));
            }
        }
    }

    /// <summary>
    /// Creates bindings for properties of a complex type.
    /// </summary>
    /// <param name="type">Declaring complex type.</param>
    /// <param name="properties">Property names to bind.</param>
    /// <param name="paramExpr">Source parameter/member expression.</param>
    /// <returns>Array of member assignments.</returns>
    /// <exception cref="ArgumentException">Thrown when a property is not found.</exception>
    private static MemberAssignment[] CreateMemberBindingHelper(Type type, List<string> properties, Expression paramExpr)
    {
        return properties
            .Select(p =>
            {
                var propertyInfo = type.GetProperty(p);
                if (propertyInfo == null)
                    throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(p, type.Name));

                var propertyExpr = Expression.Property(paramExpr, propertyInfo);
                return Expression.Bind(propertyInfo, propertyExpr);
            }).ToArray();
    }

    /// <summary>
    /// Creates a binding for a collection property by projecting item fields and materializing to a list.
    /// </summary>
    /// <param name="type">Collection type.</param>
    /// <param name="properties">Item property names.</param>
    /// <param name="paramExpr">Collection expression.</param>
    /// <returns>Select followed by ToList call expression.</returns>
    private static MethodCallExpression CreateListBindingHelper(Type type, List<string> properties, Expression paramExpr)
    {
        var itemType = type.IsGenericType ? type.GetGenericArguments()[0] : type.GetElementType();

        var itemParam = Expression.Parameter(itemType!, GetNextParameter());

        var itemBindings = CreateMemberBindingHelper(itemType!, properties, itemParam);

        var newItem = Expression.MemberInit(Expression.New(itemType!), itemBindings);
        var selectLambda = Expression.Lambda(newItem, itemParam);

        return CreateSelectCall(itemType!, selectLambda, paramExpr);
    }

    /// <summary>
    /// Builds Enumerable.Select followed by Enumerable.ToList for a collection.
    /// </summary>
    /// <param name="type">Item type.</param>
    /// <param name="lambda">Projection lambda.</param>
    /// <param name="paramExpr">Collection expression.</param>
    /// <returns>Method call expression that materializes to a list.</returns>
    private static MethodCallExpression CreateSelectCall(Type type, LambdaExpression lambda, Expression paramExpr)
    {
        var selectMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Select" && m.GetParameters().Length == 2)
            .MakeGenericMethod(type!, type!);

        var selectCall = Expression.Call(selectMethod, paramExpr, lambda);

        var toListMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "ToList" && m.GetParameters().Length == 1)
            .MakeGenericMethod(type!);

        return Expression.Call(toListMethod, selectCall);
    }

    /// <summary>
    /// Initializes available parameter names (a..z).
    /// </summary>
    private static void InitializeAvaiableParameters()
    {
        _avaibleParameters = Enumerable.Range('a', 26).Select(c => ((char)c).ToString()).ToList();
    }

    /// <summary>
    /// Gets the next available parameter name.
    /// </summary>
    /// <returns>Next name.</returns>
    /// <exception cref="InvalidOperationException">Thrown when pool is exhausted.</exception>
    private static string GetNextParameter()
    {
        var proximoParametro = _avaibleParameters.FirstOrDefault();
        _avaibleParameters = _avaibleParameters.Skip(1).ToList();
        return proximoParametro ?? throw new InvalidOperationException(DomainMessages.NoMoreParametersAvailable);
    }
}