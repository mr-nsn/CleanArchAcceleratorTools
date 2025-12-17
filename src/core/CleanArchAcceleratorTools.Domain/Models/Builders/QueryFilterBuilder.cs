using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Domain.Models.Builders;

/// <summary>
/// Fluent builder for <see cref="QueryFilter{TEntity}"/> supporting pagination, field selection, dynamic filter, and sort.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
public class QueryFilterBuilder<TEntity> where TEntity : Entity
{
    private readonly QueryFilter<TEntity> _queryFilter;

    /// <summary>
    /// Initializes the builder.
    /// </summary>
    public QueryFilterBuilder()
    {
        _queryFilter = new QueryFilter<TEntity>();
    }

    /// <summary>
    /// Sets the page number.
    /// </summary>
    /// <param name="page">1-based page (>= 1).</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="page"/> &lt; 1.</exception>
    public QueryFilterBuilder<TEntity> WithPage(int page)
    {
        _queryFilter.SetPage(page);
        return this;
    }

    /// <summary>
    /// Sets the page size.
    /// </summary>
    /// <param name="pageSize">Items per page (>= 1).</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="pageSize"/> &lt; 1.</exception>
    public QueryFilterBuilder<TEntity> WithPageSize(int pageSize)
    {
        _queryFilter.SetPageSize(pageSize);
        return this;
    }

    /// <summary>
    /// Sets the dynamic filter.
    /// </summary>
    /// <param name="filter">Filter definition. Cannot be null.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="filter"/> is null.</exception>
    public QueryFilterBuilder<TEntity> WithDynamicFilter(DynamicFilter<TEntity> filter)
    {
        _queryFilter.SetDynamicFilter(filter);
        return this;
    }

    /// <summary>
    /// Sets the dynamic sort.
    /// </summary>
    /// <param name="sort">Sort definition. Cannot be null.</param>
    /// <returns>The current builder instance.</returns>
    public QueryFilterBuilder<TEntity> WithDynamicSort(DynamicSort<TEntity> sort)
    {
        _queryFilter.SetDynamicSort(sort);
        return this;
    }

    /// <summary>
    /// Sets the field projection.
    /// </summary>
    /// <param name="fields">Field names to include. Cannot be null or empty.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="fields"/> is null or empty.</exception>
    public QueryFilterBuilder<TEntity> WithFields(params string[] fields)
    {
        _queryFilter.SetFields(fields);
        return this;
    }

    /// <summary>
    /// Builds the configured <see cref="QueryFilter{TEntity}"/>.
    /// </summary>
    /// <returns>The constructed <see cref="QueryFilter{TEntity}"/>.</returns>
    public QueryFilter<TEntity> Build()
    {
        return _queryFilter;
    }
}