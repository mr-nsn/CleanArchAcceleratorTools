using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Util;
using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Domain.DynamicSorts;

/// <summary>
/// Builds LINQ sort selector expressions from <see cref="FieldSort{TEntity}"/> definitions.
/// </summary>
/// <remarks>
/// Produces selector + order ("asc"/"desc") for OrderBy/ThenBy (incl. descending) and supports nested paths.
/// Defaults to <see cref="Entity.Id"/> descending when no fields are provided.
/// </remarks>
internal static class DynamicSortHelper
{
    /// <summary>
    /// Pool of parameter names for readable, deterministic expressions (a..z).
    /// </summary>
    private static List<string> _avaibleParameters = Enumerable.Empty<string>().ToList();

    /// <summary>
    /// Creates selector expressions and their corresponding order directions.
    /// </summary>
    /// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
    /// <param name="fieldOrders">Field paths with sort direction.</param>
    /// <returns>Tuples of selector and order ("asc"/"desc").</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="fieldOrders"/> is null.</exception>
    /// <exception cref="InvalidOperationException">When a field path is invalid or missing.</exception>
    public static ICollection<(Expression<Func<TEntity, object?>> Expression, string Order)> BuildSort<TEntity>(ICollection<FieldSort<TEntity>> fieldOrders) where TEntity : Entity
    {
        InitializeAvaiableParameters();

        var orders = new List<(Expression<Func<TEntity, object?>>, string)>();

        if (fieldOrders is null || !fieldOrders.Any())
        {
            orders.Add((x => x.Id, DynamicSortConstants.SORT_ORDER_DESC));
            return orders;
        }

        // Entity root parameter
        var entityParameter = Expression.Parameter(typeof(TEntity), GetNextParameter());

        // Build expressions for each field order
        foreach (var fieldOrder in fieldOrders)
        {
            var order = BuildFieldSort(entityParameter, fieldOrder);

            if (order is not null) orders.Add(order.Value);
        }

        return orders;
    }

    /// <summary>
    /// Builds a selector for a simple or nested field path.
    /// </summary>
    /// <exception cref="InvalidOperationException">When the field path is invalid.</exception>
    private static (Expression<Func<TEntity, object?>> Expression, string Order)? BuildFieldSort<TEntity>(ParameterExpression parameter, FieldSort<TEntity> fieldSort) where TEntity : Entity
    {
        var splitFields = fieldSort.Field.Split('.');

        if (splitFields.Length == 1)
        {
            var property = typeof(TEntity).GetProperty(fieldSort.Field);

            if (property is null)
                throw new InvalidOperationException(DomainMessages.FieldDoesNotExistInEntity.ToFormat(typeof(TEntity).Name, fieldSort.Field));

            var memberExp = Expression.Property(parameter, property.Name);
            return (Expression.Lambda<Func<TEntity, object?>>(Expression.Convert(memberExp, typeof(object)), parameter), fieldSort.Order);
        }
        else if (splitFields.Length > 1)
        {
            var property = typeof(TEntity).GetProperty(splitFields[0]);

            if (property is null)
                throw new InvalidOperationException(DomainMessages.FieldDoesNotExistInEntity.ToFormat(typeof(TEntity).Name, fieldSort.Field));

            var memberExp = Expression.Property(parameter, property.Name);
            return BuildFieldSortRecursive(parameter, fieldSort, memberExp, property.PropertyType, splitFields.Skip(1).ToArray());
        }

        return null;
    }

    /// <summary>
    /// Recursively resolves a nested path into a selector expression.
    /// </summary>
    /// <exception cref="InvalidOperationException">When the nested path cannot be resolved.</exception>
    private static (Expression<Func<TEntity, object?>> Expression, string Order)? BuildFieldSortRecursive<TEntity>(ParameterExpression parameter, FieldSort<TEntity> fieldSort, MemberExpression memberExp, Type type, string[] fields) where TEntity : Entity
    {
        if (fields.Length < 1) return null;

        if (fields.Length == 1)
        {
            var property = type.GetProperty(fields[0]);

            if (property is null)
                throw new InvalidOperationException(DomainMessages.FieldDoesNotExistInEntity.ToFormat(typeof(TEntity).Name, fieldSort.Field));

            var nextMemberExp = Expression.Property(memberExp, property.Name);
            return (Expression.Lambda<Func<TEntity, object?>>(nextMemberExp, parameter), fieldSort.Order);
        }
        else if (fields.Length == 2)
        {
            var property = type.GetProperty(fields[0]);

            if (property is null)
                throw new InvalidOperationException(DomainMessages.FieldDoesNotExistInEntity.ToFormat(typeof(TEntity).Name, fieldSort.Field));

            var nextMemberExp = Expression.Property(Expression.Property(memberExp, property.Name), fields[1]);

            return (Expression.Lambda<Func<TEntity, object?>>(nextMemberExp, parameter), fieldSort.Order);
        }
        else if (fields.Length > 2)
        {
            var property = type.GetProperty(fields[0]);

            if (property is null)
                throw new InvalidOperationException(DomainMessages.FieldDoesNotExistInEntity.ToFormat(typeof(TEntity).Name, fieldSort.Field));

            var nextProperty = property.PropertyType.GetProperty(fields[1]);

            if (nextProperty is null)
                throw new InvalidOperationException(DomainMessages.FieldDoesNotExistInEntity.ToFormat(typeof(TEntity).Name, fieldSort.Field));

            var nextMemberExp = Expression.Property(Expression.Property(memberExp, property.Name), fields[1]);

            return BuildFieldSortRecursive(parameter, fieldSort, nextMemberExp, nextProperty.PropertyType, fields.Skip(2).ToArray());
        }

        return null;
    }

    /// <summary>
    /// Initializes the a..z parameter name pool.
    /// </summary>
    private static void InitializeAvaiableParameters()
    {
        _avaibleParameters = Enumerable.Range('a', 26).Select(c => ((char)c).ToString()).ToList();
    }

    /// <summary>
    /// Returns the next parameter name; throws if none left.
    /// </summary>
    private static string GetNextParameter()
    {
        var proximoParametro = _avaibleParameters.FirstOrDefault();
        _avaibleParameters = _avaibleParameters.Skip(1).ToList();
        return proximoParametro ?? throw new InvalidOperationException(DomainMessages.NoMoreParametersAvailable);
    }
}