using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Models.Builders;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAcceleratorTools.Infrastructure.Pagination;

/// <summary>
/// Extensions to apply pagination and materialize paged results.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Applies pagination based on the given <see cref="QueryFilter{T}"/> and returns a <see cref="PaginationResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Item type; must inherit from <see cref="Entity"/>.</typeparam>
    /// <param name="query">Source queryable sequence.</param>
    /// <param name="queryFilter">Filter with page and page size.</param>
    /// <returns>A <see cref="PaginationResult{T}"/> holding metadata and the paginated query.</returns>
    public static PaginationResult<T> GetPagination<T>(this IQueryable<T> query, QueryFilter<T> queryFilter) where T : Entity
    {
        var totalRecords = query.Count();
        int skip = (queryFilter.Page - 1) * queryFilter.PageSize;
        var paginatedQuery = query
            .Skip(skip)
            .Take(queryFilter.PageSize);

        return new PaginationResultBuilder<T>()
            .WithPage(queryFilter.Page)
            .WithPageSize(queryFilter.PageSize)
            .WithTotalRecords(totalRecords)
            .WithQuery(paginatedQuery)
            .Build();
    }

    /// <summary>
    /// Asynchronously materializes the paginated query into <see cref="PaginationResult{T}.Result"/>.
    /// </summary>
    /// <typeparam name="T">Item type; must inherit from <see cref="Entity"/>.</typeparam>
    /// <param name="pagination">Container whose <see cref="PaginationResult{T}.Query"/> will be executed.</param>
    /// <returns>A task that completes with the same <see cref="PaginationResult{T}"/> populated with items.</returns>
    public static async Task<PaginationResult<T>> ToPaginationResultListAsync<T>(this PaginationResult<T> pagination) where T : Entity
    {
        pagination.SetResult(await pagination.Query.ToListAsync());
        return pagination;
    }

    /// <summary>
    /// Synchronously materializes the paginated query into <see cref="PaginationResult{T}.Result"/>.
    /// </summary>
    /// <typeparam name="T">Item type; must inherit from <see cref="Entity"/>.</typeparam>
    /// <param name="pagination">Container whose <see cref="PaginationResult{T}.Query"/> will be executed.</param>
    /// <returns>The same <see cref="PaginationResult{T}"/> populated with items.</returns>
    public static PaginationResult<T> ToPaginationResultList<T>(this PaginationResult<T> pagination) where T : Entity
    {
        pagination.SetResult(pagination.Query.ToList());
        return pagination;
    }
}
