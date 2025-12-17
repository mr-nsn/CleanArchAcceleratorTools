namespace CleanArchAcceleratorTools.Domain.Models.Builders;

/// <summary>
/// Fluent builder for <see cref="DynamicSort{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Configures sort fields and directions, returning a sort definition ready for validation and LINQ compilation.
/// </remarks>
public class DynamicSortBuilder<TEntity> where TEntity : Entity
{
    private readonly DynamicSort<TEntity> _dynamicSort;

    /// <summary>
    /// Initializes the sort builder.
    /// </summary>
    public DynamicSortBuilder()
    {
        _dynamicSort = new DynamicSort<TEntity>();
    }

    /// <summary>
    /// Replaces the sort field orders.
    /// </summary>
    /// <param name="fieldsSort">Collection of field orders (field name and direction).</param>
    /// <returns>The current builder instance.</returns>
    public DynamicSortBuilder<TEntity> WithFieldsSort(ICollection<FieldSort<TEntity>> fieldsSort)
    {
        _dynamicSort.SetFieldsSort(fieldsSort);
        return this;
    }

    /// <summary>
    /// Adds a single field order.
    /// </summary>
    /// <param name="fieldSort">Field path and sort direction.</param>
    /// <returns>The current builder instance.</returns>
    public DynamicSortBuilder<TEntity> AddFieldOrder(FieldSort<TEntity> fieldSort)
    {
        _dynamicSort.AddFieldSort(fieldSort.Field, fieldSort.Order);
        return this;
    }

    /// <summary>
    /// Builds the configured sort definition.
    /// </summary>
    /// <returns>The constructed <see cref="DynamicSort{TEntity}"/>.</returns>
    public DynamicSort<TEntity> Build()
    {
        return _dynamicSort;
    }
}

/// <summary>
/// Fluent builder for <see cref="FieldSort{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Configures target field path and sort direction ("asc"/"desc").
/// </remarks>
public class FieldOrderBuilder<TEntity> where TEntity : Entity
{
    private readonly FieldSort<TEntity> _fieldOrder;

    /// <summary>
    /// Initializes the field order builder.
    /// </summary>
    public FieldOrderBuilder()
    {
        _fieldOrder = new FieldSort<TEntity>();
    }

    /// <summary>
    /// Sets the field path to sort.
    /// </summary>
    /// <param name="field">Field name or dot-notated path (e.g., "Name", "Address.City").</param>
    /// <returns>The current builder instance.</returns>
    public FieldOrderBuilder<TEntity> WithField(string field)
    {
        _fieldOrder.SetField(field);
        return this;
    }

    /// <summary>
    /// Sets the sort direction.
    /// </summary>
    /// <param name="order">Sort direction, typically "asc" or "desc".</param>
    /// <returns>The current builder instance.</returns>
    public FieldOrderBuilder<TEntity> WithOrder(string order)
    {
        _fieldOrder.SetOrder(order);
        return this;
    }

    /// <summary>
    /// Builds the configured field order.
    /// </summary>
    /// <returns>The constructed <see cref="FieldSort{TEntity}"/>.</returns>
    public FieldSort<TEntity> Build()
    {
        return _fieldOrder;
    }
}