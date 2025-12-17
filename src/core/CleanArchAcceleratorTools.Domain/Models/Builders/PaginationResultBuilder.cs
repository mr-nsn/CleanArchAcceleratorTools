namespace CleanArchAcceleratorTools.Domain.Models.Builders;

/// <summary>
/// Fluent builder for <see cref="PaginationResult{T}"/> with pagination metadata and items.
/// </summary>
/// <typeparam name="T">Item type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Wraps the validations enforced by <see cref="PaginationResult{T}"/> setters.
/// </remarks>
public class PaginationResultBuilder<T> where T : Entity
{
    private readonly PaginationResult<T> _paginationResult;

    /// <summary>
    /// Initializes the builder with a default <see cref="PaginationResult{T}"/>.
    /// </summary>
    public PaginationResultBuilder()
    {
        _paginationResult = new PaginationResult<T>();
    }

    /// <summary>
    /// Sets the current page number.
    /// </summary>
    /// <param name="page">Page number (>= 1).</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="page"/> &lt; 1.</exception>
    public PaginationResultBuilder<T> WithPage(int page)
    {
        _paginationResult.SetPage(page);
        return this;
    }

    /// <summary>
    /// Sets the page size (items per page).
    /// </summary>
    /// <param name="pageSize">Items per page (>= 1).</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="pageSize"/> &lt; 1.</exception>
    public PaginationResultBuilder<T> WithPageSize(int pageSize)
    {
        _paginationResult.SetPageSize(pageSize);
        return this;
    }

    /// <summary>
    /// Sets the total number of records.
    /// </summary>
    /// <param name="totalRecords">Total records (>= 0).</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="totalRecords"/> is negative.</exception>
    public PaginationResultBuilder<T> WithTotalRecords(int totalRecords)
    {
        _paginationResult.SetTotalRecords(totalRecords);
        return this;
    }

    /// <summary>
    /// Sets the source query.
    /// </summary>
    /// <param name="query"><see cref="IQueryable{T}"/> data source. Cannot be null.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="query"/> is null.</exception>
    public PaginationResultBuilder<T> WithQuery(IQueryable<T> query)
    {
        _paginationResult.SetQuery(query);
        return this;
    }

    /// <summary>
    /// Sets the result items for the current page.
    /// </summary>
    /// <param name="result">Items for the current page, or <c>null</c> to materialize from the query.</param>
    /// <returns>The current builder instance.</returns>
    public PaginationResultBuilder<T> WithResult(ICollection<T> result)
    {
        _paginationResult.SetResult(result);
        return this;
    }

    /// <summary>
    /// Builds the configured <see cref="PaginationResult{T}"/>.
    /// </summary>
    /// <returns>The constructed <see cref="PaginationResult{T}"/>.</returns>
    public PaginationResult<T> Build()
    {
        return _paginationResult;
    }
}