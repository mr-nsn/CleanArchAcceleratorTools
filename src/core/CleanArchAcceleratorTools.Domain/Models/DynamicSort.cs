using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.DynamicSorts;
using CleanArchAcceleratorTools.Domain.Exceptions;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models.Validators;
using CleanArchAcceleratorTools.Domain.Util;
using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Domain.Models;

/// <summary>
/// Compact dynamic sort definition for building LINQ order selectors.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Use <see cref="CompileSort"/> to translate field orders into selector expressions for LINQ providers.
/// </remarks>
public class DynamicSort<TEntity> : Entity where TEntity : Entity
{
    /// <summary>
    /// Field orders composing the sort definition.
    /// </summary>
    public ICollection<FieldSort<TEntity>> FieldsSort { get; private set; }

    /// <summary>
    /// Initializes with no field orders.
    /// </summary>
    public DynamicSort()
    {
        FieldsSort = new List<FieldSort<TEntity>>();
    }

    /// <summary>
    /// Adds a field path and sort direction.
    /// </summary>
    /// <param name="field">Field name or dot-notated path (e.g., "Name", "Address.City").</param>
    /// <param name="order">Sort direction, typically <c>asc</c> or <c>desc</c>.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="field"/> or <paramref name="order"/> is null or whitespace.</exception>
    public void AddFieldSort(string field, string order)
    {
        if (field is null) throw new ArgumentNullException(nameof(field), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(field)));
        if (string.IsNullOrWhiteSpace(field)) throw new ArgumentNullException(nameof(field), DomainMessages.PropertyCannotBeEmpty.ToFormat(nameof(field)));
        if (order is null) throw new ArgumentNullException(nameof(order), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(order)));
        if (string.IsNullOrWhiteSpace(order)) throw new ArgumentNullException(nameof(order), DomainMessages.PropertyCannotBeEmpty.ToFormat(nameof(order)));

        var fieldOrder = new FieldSort<TEntity>();
        fieldOrder.SetField(field);
        fieldOrder.SetOrder(order);
        FieldsSort.Add(fieldOrder);
    }

    /// <summary>
    /// Replaces the current field orders.
    /// </summary>
    /// <param name="fieldsSort">New collection of field orders.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fieldsSort"/> is null.</exception>
    public void SetFieldsSort(ICollection<FieldSort<TEntity>> fieldsSort)
    {
        if (fieldsSort is null) throw new ArgumentNullException(nameof(fieldsSort), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(fieldsSort)));
        FieldsSort = fieldsSort;
    }

    /// <summary>
    /// Compiles the sort into selector expressions paired with order.
    /// </summary>
    /// <returns>A collection of (selector, order) tuples, where order is "asc" or "desc".</returns>
    /// <exception cref="InvalidOperationException">Thrown when the sort is invalid (see <see cref="IsValid"/>).</exception>
    public ICollection<(Expression<Func<TEntity, object?>> Expression, string Order)> CompileSort()
    {
        if (!Validate(new DynamicSortValidator<TEntity>())) throw new DomainException(ValidationResult);
        return DynamicSortHelper.BuildSort(FieldsSort);
    }
}

/// <summary>
/// Single field sort entry (field path + direction).
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// <see cref="Order"/> must be <see cref="DynamicSortConstants.SORT_ORDER_ASC"/> or <see cref="DynamicSortConstants.SORT_ORDER_DESC"/>.
/// </remarks>
public class FieldSort<TEntity> where TEntity : Entity
{
    /// <summary>
    /// Target field name or nested path (dot notation).
    /// </summary>
    public string Field { get; private set; }

    /// <summary>
    /// Sort direction ("asc" or "desc").
    /// </summary>
    public string Order { get; private set; }

    /// <summary>
    /// Initializes with default field <see cref="Entity.Id"/> and ascending order.
    /// </summary>
    public FieldSort()
    {
        Field = nameof(Entity.Id);
        Order = DynamicSortConstants.SORT_ORDER_ASC;
    }

    /// <summary>
    /// Sets the field name or path.
    /// </summary>
    /// <param name="field">Field or dot-notated path.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="field"/> is null.</exception>
    public void SetField(string field)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(field)));
    }

    /// <summary>
    /// Sets the sort direction.
    /// </summary>
    /// <param name="order">Sort direction, typically "asc" or "desc".</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="order"/> is null.</exception>
    public void SetOrder(string order)
    {
        Order = order ?? throw new ArgumentNullException(nameof(order), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(order)));
    }
}