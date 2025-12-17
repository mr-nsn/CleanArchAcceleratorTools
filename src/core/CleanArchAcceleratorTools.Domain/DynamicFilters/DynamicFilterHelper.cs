using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Types;
using CleanArchAcceleratorTools.Domain.Util;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Domain.DynamicFilters;

/// <summary>
/// Builds LINQ predicate expressions from <see cref="DynamicFilter{TEntity}"/>.
/// </summary>
/// <remarks>
/// - Quick search: OR over properties marked with <see cref="QuickSearchAttribute"/>.
/// - Clause groups: AND/OR per <see cref="DynamicFilterConstants"/>.
/// - Supports nested fields via dot notation.
/// - Uses an internal a..z parameter pool; not thread-safe across concurrent calls.
/// </remarks>
internal static class DynamicFilterHelper
{
    /// <summary>Pool of parameter names to keep expressions readable (a..z).</summary>
    private static List<string> _avaibleParameters = Enumerable.Empty<string>().ToList();

    /// <summary>
    /// Translates a <see cref="DynamicFilter{TEntity}"/> into <c>Expression&lt;Func&lt;TEntity,bool&gt;&gt;</c>.
    /// </summary>
    /// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
    /// <param name="dynamicFilter">Filter containing quick search and clause groups.</param>
    /// <returns>Predicate expression for LINQ Where.</returns>
    /// <exception cref="ArgumentException">Invalid field or operator.</exception>
    public static Expression<Func<TEntity, bool>> BuildFilter<TEntity>(DynamicFilter<TEntity> dynamicFilter) where TEntity : Entity
    {
        InitializeAvaiableParameters();

        // Root predicate (initiates with true to guarantee the result if any filter were passed)
        Expression quickSearchPredicate = Expression.Constant(true);
        Expression clauseGroupPredicate = Expression.Constant(true);

        // Entity root parameter
        var entityParameter = Expression.Parameter(typeof(TEntity), GetNextParameter());

        // Build quick search binary expressions
        var quickSearchExps = !string.IsNullOrWhiteSpace(dynamicFilter.QuickSearch)
            ? BuildQuickSearchExp<TEntity>(entityParameter, dynamicFilter.QuickSearch)
            : new List<Expression>();

        if (quickSearchExps is not null && quickSearchExps.Any())
        {
            // Root quick search OR expression (fallback to ensure the right operator will be checked)
            Expression quickSearchOrExp = Expression.Constant(false);

            // Join all quick search binary expressions with OR
            foreach (var quickSearchExp in quickSearchExps)
                quickSearchOrExp = Expression.OrElse(quickSearchOrExp, quickSearchExp);

            // Join quick search expression to the main predicate with AND
            quickSearchPredicate = Expression.AndAlso(quickSearchPredicate, quickSearchOrExp);
        }

        // Build clause group binary expressions
        var clauseGroupsTuples = dynamicFilter.ClauseGroups is not null && dynamicFilter.ClauseGroups.Any()
            ? BuildClauseGroupExp(entityParameter, dynamicFilter.ClauseGroups)
            : new List<(Expression ClauseGroupExp, string LogicOperator)>();

        if (clauseGroupsTuples is not null && clauseGroupsTuples.Any())
        {
            foreach (var clauseGroupTuple in clauseGroupsTuples)
            {
                switch (clauseGroupTuple.LogicOperator)
                {
                    case DynamicFilterConstants.LOGIC_OPERATOR_AND:
                        clauseGroupPredicate = Expression.AndAlso(clauseGroupPredicate, clauseGroupTuple.ClauseGroupExp);
                        break;
                    case DynamicFilterConstants.LOGIC_OPERATOR_OR:
                        clauseGroupPredicate = Expression.OrElse(clauseGroupPredicate, clauseGroupTuple.ClauseGroupExp);
                        break;
                }
            }
        }

        return Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(quickSearchPredicate, clauseGroupPredicate), entityParameter);
    }

    /// <summary>
    /// Builds quick search comparisons for properties annotated with <see cref="QuickSearchAttribute"/>.
    /// </summary>
    private static List<Expression> BuildQuickSearchExp<TEntity>(ParameterExpression entityParameter, string? quickSearch) where TEntity : Entity
    {
        var returnExp = new List<Expression>();

        if (string.IsNullOrWhiteSpace(quickSearch))
        {
            returnExp.Add(Expression.Equal(Expression.Constant(true), Expression.Constant(true)));
            return returnExp;
        }

        var filteredProperties = typeof(TEntity)
            .GetProperties()
            .SelectMany(x => x.GetCustomAttributes(false).OfType<QuickSearchAttribute>(), (prop, attr) => new { PropertyInfo = prop, Attribute = attr });

        foreach (var property in filteredProperties)
        {
            var comparisonOperator = property.Attribute.ComparisonOperator;

            if (!CleanArchTypeExtensions.IsComplex(property.PropertyInfo.PropertyType))
            {
                var fieldName = property.PropertyInfo.Name;
                var fieldType = property.PropertyInfo.PropertyType;
                var memberExp = Expression.Property(entityParameter, fieldName);
                var quickSearchExp = MakeExpression(fieldName, fieldType, comparisonOperator, memberExp, quickSearch);

                if (quickSearchExp == null) continue;

                returnExp.Add(quickSearchExp);
            }
            else
            {
                var nestedFieldName = property.PropertyInfo.Name;
                var nestedFieldType = property.PropertyInfo.PropertyType;
                var nestedMemberExp = Expression.Property(entityParameter, nestedFieldName);
                var nestedQuickSearchExp = BuildQuickSearchExpRecursive(nestedFieldName, nestedFieldType, comparisonOperator, entityParameter, nestedMemberExp, quickSearch);

                if (nestedQuickSearchExp == null) continue;

                returnExp.AddRange(nestedQuickSearchExp);
            }
        }

        return returnExp;
    }

    /// <summary>
    /// Creates a comparison expression for a member and value using the specified operator.
    /// </summary>
    /// <exception cref="ArgumentException">Unsupported operators or invalid types for string operators.</exception>
    private static Expression? MakeExpression(string fieldName, Type fieldType, string comparisonOperator, MemberExpression memberExp, object? constantValue)
    {
        if (constantValue is null) return null;

        var convertedValue = ConvertToTypeFromString(constantValue, fieldType);

        if (convertedValue is null) return null;

        ConstantExpression valueExp = Expression.Constant(convertedValue, fieldType);

        Expression? expression = null;

        switch (comparisonOperator)
        {
            case DynamicFilterConstants.COMPARISON_OPERATOR_EQUAL:
                expression = Expression.Equal(memberExp, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_NOT_EQUAL:
                expression = Expression.NotEqual(memberExp, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_GREATER_THAN:
                expression = Expression.GreaterThan(memberExp, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_GREATER_THAN_OR_EQUAL:
                expression = Expression.GreaterThanOrEqual(memberExp, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_LESS_THAN:
                expression = Expression.LessThan(memberExp, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_LESS_THAN_OR_EQUAL:
                expression = Expression.LessThanOrEqual(memberExp, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_LIKE:
                if (fieldType != typeof(string)) throw new ArgumentException(DomainMessages.OperatorOnlyForStringFields.ToFormat(comparisonOperator, fieldName, fieldType.Name));
                var methodInfoLike = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                expression = Expression.Call(memberExp, methodInfoLike!, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_NOT_LIKE:
                var methodInfoNotLike = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                expression = Expression.Not(Expression.Call(memberExp, methodInfoNotLike!, valueExp));
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_STARTS_WITH:
                if (fieldType != typeof(string)) throw new ArgumentException(DomainMessages.OperatorOnlyForStringFields.ToFormat(comparisonOperator, fieldName, fieldType.Name));
                var methodInfoStartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                expression = Expression.Call(memberExp, methodInfoStartsWith!, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_NOT_STARTS_WITH:
                if (fieldType != typeof(string)) throw new ArgumentException(DomainMessages.OperatorOnlyForStringFields.ToFormat(comparisonOperator, fieldName, fieldType.Name));
                var methodInfoNotStartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                expression = Expression.Not(Expression.Call(memberExp, methodInfoNotStartsWith!, valueExp));
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_ENDS_WITH:
                if (fieldType != typeof(string)) throw new ArgumentException(DomainMessages.OperatorOnlyForStringFields.ToFormat(comparisonOperator, fieldName, fieldType.Name));
                var methodInfoEndsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                expression = Expression.Call(memberExp, methodInfoEndsWith!, valueExp);
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_NOT_ENDS_WITH:
                if (fieldType != typeof(string)) throw new ArgumentException(DomainMessages.OperatorOnlyForStringFields.ToFormat(comparisonOperator, fieldName, fieldType.Name));
                var methodInfoNotEndsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                expression = Expression.Not(Expression.Call(memberExp, methodInfoNotEndsWith!, valueExp));
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_IS_EMPTY:
                expression = Expression.Equal(memberExp, Expression.Constant(string.Empty));
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_IS_NOT_EMPTY:
                expression = Expression.NotEqual(memberExp, Expression.Constant(string.Empty));
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_IN:
                var splitedValuesIn = convertedValue.ToString()?.Split(',').Select(v => v.Trim()).ToList();
                if (splitedValuesIn?.Count >= 1)
                {
                    var newListExp = Expression.New(typeof(List<string>));
                    var listInitExp = Expression.ListInit(newListExp, splitedValuesIn.Select(v => Expression.Constant(v)));
                    var methodInfoIn = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
                    expression = Expression.Call(listInitExp, methodInfoIn!, memberExp);
                }
                break;
            case DynamicFilterConstants.COMPARISON_OPERATOR_NOT_IN:
                var splitedValuesNotIn = convertedValue.ToString()?.Split(',').Select(v => v.Trim()).ToList();
                if (splitedValuesNotIn?.Count >= 1)
                {
                    var listExp = Expression.New(typeof(List<string>));
                    var listInitExp = Expression.ListInit(listExp, splitedValuesNotIn.Select(v => Expression.Constant(v)));
                    var methodInfoNotIn = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
                    expression = Expression.Not(Expression.Call(listInitExp, methodInfoNotIn!, memberExp));
                }
                break;
            default:
                throw new ArgumentException(DomainMessages.InvalidComparisonOperator.ToFormat(comparisonOperator));
        }

        return expression;
    }

    /// <summary>
    /// Recursively creates quick search expressions for nested complex properties.
    /// </summary>
    private static List<Expression>? BuildQuickSearchExpRecursive(string fieldName, Type fieldType, string comparisonOperator, ParameterExpression parameter, MemberExpression memberExp, object? constantValue)
    {
        var returnExp = new List<Expression>();

        if (CleanArchTypeExtensions.IsComplex(fieldType))
        {
            var filteredProperties = fieldType
                .GetProperties()
                .SelectMany(x => x.GetCustomAttributes(false).OfType<QuickSearchAttribute>(), (prop, attr) => new { PropertyInfo = prop, Attribute = attr });

            foreach (var property in filteredProperties)
            {
                var nestedLogicOperator = property.Attribute.ComparisonOperator;
                var nestedFieldName = property.PropertyInfo.Name;
                var nestedFieldType = property.PropertyInfo.PropertyType;
                var nestedMemberExp = Expression.Property(memberExp, nestedFieldName);

                var exp = BuildQuickSearchExpRecursive(nestedFieldName, nestedFieldType, nestedLogicOperator, parameter, nestedMemberExp, constantValue);

                if (exp != null) returnExp.AddRange(exp);
            }
        }
        else
        {
            var exp = MakeExpression(fieldName, fieldType, comparisonOperator, memberExp, constantValue);

            if (exp != null) returnExp.Add(exp);
        }

        return returnExp;
    }

    /// <summary>
    /// Converts a value to the target CLR type using <see cref="TypeConverter"/>; returns null if it fails.
    /// </summary>
    private static object? ConvertToTypeFromString(object value, Type type)
    {
        try
        {
            TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
            return typeConverter.ConvertFrom(value) ?? null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Translates each clause group into an expression and pairs it with its logical operator.
    /// </summary>
    private static List<(Expression ClauseGroupExp, string LogicOperator)> BuildClauseGroupExp<TEntity>(ParameterExpression entityParameter, ICollection<ClauseGroup<TEntity>> clauseGroups) where TEntity : Entity
    {
        var groupsExpsTuple = new List<(Expression ClauseGroupExp, string LogicOperator)>();

        if (clauseGroups is null || !clauseGroups.Any())
        {
            var trueExp = Expression.Equal(Expression.Constant(true), Expression.Constant(true));
            groupsExpsTuple.Add((trueExp, DynamicFilterConstants.LOGIC_OPERATOR_AND));
            return groupsExpsTuple;
        }

        foreach (var clauseGroup in clauseGroups)
        {
            var groupExps = new List<(Expression ClauseGroupExp, string LogicOperator)>();

            foreach (var clause in clauseGroup.Clauses)
            {
                var splitedFields = clause.Field.Split('.');

                if (splitedFields.Length == 1)
                {
                    var property = typeof(TEntity).GetProperty(clause.Field);

                    if (property is null) throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(clause.Field, typeof(TEntity).Name));
                    if (CleanArchTypeExtensions.IsCollection(property.PropertyType)) throw new ArgumentException(DomainMessages.CollectionsNotSupportedFor.ToFormat(nameof(clause)));

                    var fieldName = property.Name;
                    var fieldType = property.PropertyType;
                    var memberExp = Expression.Property(entityParameter, fieldName);
                    var exp = MakeExpression(fieldName, fieldType, clause.ComparisonOperator, memberExp, clause.Value);

                    if (exp == null) continue;

                    groupExps.Add((exp, clause.LogicOperator));
                }
                else if (splitedFields.Length > 1)
                {
                    // Getting the new main property
                    var newMainField = splitedFields[0];
                    var newMainProperty = typeof(TEntity).GetProperty(newMainField);

                    if (newMainProperty is null) throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(clause.Field, typeof(TEntity).Name));
                    if (CleanArchTypeExtensions.IsCollection(newMainProperty.PropertyType)) throw new ArgumentException(DomainMessages.CollectionsNotSupportedFor.ToFormat(nameof(clause)));

                    var newMainType = newMainProperty.PropertyType;
                    var newMainMemberExp = Expression.Property(entityParameter, newMainProperty.Name);
                    var nextField = clause.Field.Substring(clause.Field.IndexOf('.') + 1);
                    var exp = BuildClauseGroupExpRecursive<TEntity>(clause, nextField, newMainType, newMainMemberExp);

                    if (exp == null) continue;

                    groupExps.Add((exp, clause.LogicOperator));
                }
            }

            var groupedClausesExp = groupExps.First().ClauseGroupExp;
            foreach (var groupExp in groupExps.Skip(1))
            {
                switch (groupExp.LogicOperator)
                {
                    case DynamicFilterConstants.LOGIC_OPERATOR_AND:
                        groupedClausesExp = Expression.AndAlso(groupedClausesExp, groupExp.ClauseGroupExp);
                        break;
                    case DynamicFilterConstants.LOGIC_OPERATOR_OR:
                        groupedClausesExp = Expression.OrElse(groupedClausesExp, groupExp.ClauseGroupExp);
                        break;
                }
            }

            groupsExpsTuple.Add((groupedClausesExp, clauseGroup.LogicOperator));
        }

        return groupsExpsTuple;
    }

    /// <summary>
    /// Recursively resolves a nested field path and builds the corresponding comparison expression.
    /// </summary>
    private static Expression? BuildClauseGroupExpRecursive<TEntity>(Clause<TEntity> clause, string fieldName, Type mainType, MemberExpression memberExp) where TEntity : Entity
    {
        var splitedFields = fieldName.Split('.');

        if (splitedFields.Length < 1) return null;

        if (splitedFields.Length == 1)
        {
            // Getting the main property nested property
            var nestedField = splitedFields[0];
            var nestedProperty = mainType.GetProperty(nestedField);

            if (nestedProperty is null) throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(clause.Field, typeof(TEntity).Name));

            var nestedMemberExp = Expression.Property(memberExp, nestedProperty.Name);

            return MakeExpression(nestedProperty.Name, nestedProperty.PropertyType, clause.ComparisonOperator, nestedMemberExp, clause.Value);
        }
        else if (splitedFields.Length == 2)
        {
            // Getting the new main property
            var newMainField = splitedFields[0];
            var newMainProperty = mainType.GetProperty(newMainField);

            if (newMainProperty is null) throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(clause.Field, typeof(TEntity).Name));

            var newMainType = newMainProperty.PropertyType;
            var newMainMemberExp = Expression.Property(memberExp, newMainProperty.Name);

            // Getting the new main property nested property
            var nestedField = splitedFields[1];
            var nestedProperty = newMainType.GetProperty(nestedField);

            if (nestedProperty is null) throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(clause.Field, typeof(TEntity).Name));

            var nestedMemberExp = Expression.Property(newMainMemberExp, nestedProperty.Name);

            return MakeExpression(nestedProperty.Name, nestedProperty.PropertyType, clause.ComparisonOperator, nestedMemberExp, clause.Value);
        }
        else if (splitedFields.Length > 2)
        {
            // Getting the new main property nested property
            var newMainField = splitedFields[0];
            var newMainProperty = mainType.GetProperty(newMainField);

            if (newMainProperty is null) throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(clause.Field, typeof(TEntity).Name));

            var newMainType = newMainProperty.PropertyType;

            // Getting the new main property nested property
            var nestedField = splitedFields[1];
            var nestedProperty = newMainType.GetProperty(nestedField);

            if (nestedProperty is null) throw new ArgumentException(DomainMessages.PropertyNotFoundOnType.ToFormat(clause.Field, typeof(TEntity).Name));

            var nestedMemberExp = Expression.Property(Expression.Property(memberExp, newMainProperty.Name), nestedProperty.Name);
            var nestedFieldName = string.Join('.', splitedFields.Skip(2));

            return BuildClauseGroupExpRecursive(clause, nestedFieldName, nestedProperty.PropertyType, nestedMemberExp);
        }

        return null;
    }

    /// <summary>Initializes the a..z parameter name pool.</summary>
    private static void InitializeAvaiableParameters()
    {
        _avaibleParameters = Enumerable.Range('a', 26).Select(c => ((char)c).ToString()).ToList();
    }

    /// <summary>Returns the next parameter name; throws if none left.</summary>
    private static string GetNextParameter()
    {
        var proximoParametro = _avaibleParameters.FirstOrDefault();
        _avaibleParameters = _avaibleParameters.Skip(1).ToList();
        return proximoParametro ?? throw new InvalidOperationException(DomainMessages.NoMoreParametersAvailable);
    }
}