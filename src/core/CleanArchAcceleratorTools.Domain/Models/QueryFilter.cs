using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models.Selects.Defaults;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Domain.Models;

/// <summary>
/// Compact query options for pagination, projection, dynamic filtering, and sorting.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Defaults: Page=1, PageSize=10, Fields=<see cref="SelectsDefaults{T}.BasicFields"/>, <see cref="DynamicFilter"/> and <see cref="DynamicSort"/> initialized.
/// Setters enforce: Page >= 1, PageSize >= 1, non-null filter/sort, and non-empty fields.
/// </remarks>
public class QueryFilter<TEntity> : Entity where TEntity : Entity
{
    /// <summary>
    /// Current 1-based page number.
    /// </summary>
    public int Page { get; private set; }

    /// <summary>
    /// Items per page.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Dynamic filter to compile into a predicate.
    /// </summary>
    public DynamicFilter<TEntity> DynamicFilter { get; private set; }

    /// <summary>
    /// Dynamic sort to compile into order selectors.
    /// </summary>
    public DynamicSort<TEntity> DynamicSort { get; private set; }

    /// <summary>
    /// Field names to include in the projection (defaults to simple fields of <typeparamref name="TEntity"/>).
    /// </summary>
    public string[] Fields { get; private set; }

    /// <summary>
    /// Initializes defaults for pagination, fields, filter, and sort.
    /// </summary>
    public QueryFilter()
    {
        Page = 1;
        PageSize = 10;
        Fields = SelectsDefaults<TEntity>.BasicFields;
        DynamicFilter = new DynamicFilter<TEntity>();
        DynamicSort = new DynamicSort<TEntity>();
    }

    /// <summary>
    /// Sets the page number.
    /// </summary>
    /// <param name="page">Page (>= 1).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="page"/> &lt; 1.</exception>
    public void SetPage(int page)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), DomainMessages.ValueMustBeGreaterThanOrEqualTo.ToFormat(nameof(page), "1"));
        Page = page;
    }

    /// <summary>
    /// Sets the page size.
    /// </summary>
    /// <param name="pageSize">Items per page (>= 1).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="pageSize"/> &lt; 1.</exception>
    public void SetPageSize(int pageSize)
    {
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), DomainMessages.ValueMustBeGreaterThanOrEqualTo.ToFormat(nameof(pageSize), "1"));
        PageSize = pageSize;
    }

    /// <summary>
    /// Sets the dynamic filter.
    /// </summary>
    /// <param name="filter">Filter definition. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="filter"/> is null.</exception>
    public void SetDynamicFilter(DynamicFilter<TEntity> filter)
    {
        DynamicFilter = filter ?? throw new ArgumentNullException(nameof(filter), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(filter)));
    }

    /// <summary>
    /// Sets the dynamic sort.
    /// </summary>
    /// <param name="sort">Sort definition. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="sort"/> is null.</exception>
    public void SetDynamicSort(DynamicSort<TEntity> sort)
    {
        DynamicSort = sort ?? throw new ArgumentNullException(nameof(sort), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(sort)));
    }

    /// <summary>
    /// Sets the field projection.
    /// </summary>
    /// <param name="fields">Field names to include. Cannot be null or empty.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fields"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fields"/> is null.</exception>
    public void SetFields(params string[] fields)
    {
        if (fields is null) throw new ArgumentNullException(nameof(fields), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(fields)));
        if (fields.Length == 0) throw new ArgumentException(DomainMessages.PropertyCannotBeEmpty.ToFormat(nameof(fields)), nameof(fields));
        Fields = fields;
    }
}