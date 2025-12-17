using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Domain.Models;

/// <summary>
/// Compact container for paginated results with metadata and items.
/// </summary>
/// <typeparam name="T">Item type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Defaults: Page=1, PageSize=10, TotalRecords=0, empty Query and Result.
/// Setters enforce: Page >= 1, PageSize >= 1, TotalRecords >= 0, Query != null; Result null materializes from Query.
/// </remarks>
public class PaginationResult<T> where T : Entity
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
    /// Total records across all pages.
    /// </summary>
    public int TotalRecords { get; private set; }

    /// <summary>
    /// Source query for pagination.
    /// </summary>
    public IQueryable<T> Query { get; private set; }

    /// <summary>
    /// Items of the current page.
    /// </summary>
    public ICollection<T> Result { get; private set; }

    /// <summary>
    /// Initializes with defaults (page 1, size 10, empty query/result).
    /// </summary>
    public PaginationResult()
    {
        Page = 1;
        PageSize = 10;
        TotalRecords = 0;
        Query = Enumerable.Empty<T>().AsQueryable();
        Result = Enumerable.Empty<T>().ToList();
    }

    /// <summary>
    /// Sets the page number.
    /// </summary>
    /// <param name="page">Page number (>= 1).</param>
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
    /// Sets the total records.
    /// </summary>
    /// <param name="totalRecords">Total records (>= 0).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="totalRecords"/> is negative.</exception>
    public void SetTotalRecords(int totalRecords)
    {
        if (totalRecords < 0) throw new ArgumentOutOfRangeException(nameof(totalRecords), DomainMessages.ValueMustBeGreaterThanOrEqualTo.ToFormat(nameof(totalRecords), "0"));
        TotalRecords = totalRecords;
    }

    /// <summary>
    /// Sets the source query.
    /// </summary>
    /// <param name="query">Data source query. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> is null.</exception>
    public void SetQuery(IQueryable<T> query)
    {
        Query = query ?? throw new ArgumentNullException(nameof(query), DomainMessages.PropertyCannotBeNull.ToFormat(nameof(query)));
    }

    /// <summary>
    /// Sets the page items; if null, materializes from <see cref="Query"/>.
    /// </summary>
    /// <param name="result">Items for the current page, or <c>null</c> to derive from the query.</param>
    public void SetResult(ICollection<T>? result = null)
    {
        Result = result ?? Query.ToList();
    }
}