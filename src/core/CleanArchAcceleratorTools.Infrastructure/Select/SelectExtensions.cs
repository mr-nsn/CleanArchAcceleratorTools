using CleanArchAcceleratorTools.Infrastructure.Select.Helpers;

namespace CleanArchAcceleratorTools.Infrastructure.Select;

/// <summary>
/// Extensions to project specific fields from an <see cref="IQueryable{T}"/> (supports nested paths).
/// </summary>
/// <remarks>
/// Uses <see cref="SelectHelper.DynamicSelectGenerator{T}(string[])"/> to build the projection.
/// <typeparamref name="T"/> must have a public parameterless constructor and writable selected properties.
/// Ensure stable ordering if combining with pagination.
/// </remarks>
public static class SelectExtensions
{
    /// <summary>
    /// Projects elements including only the specified fields.
    /// </summary>
    /// <typeparam name="T">Source element type; must be a class.</typeparam>
    /// <param name="query">Queryable source.</param>
    /// <param name="fields">Field names (dot notation supported).</param>
    /// <returns>An <see cref="IQueryable{T}"/> yielding instances with only requested fields populated.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="fields"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when a property path is invalid.</exception>
    public static IQueryable<T> DynamicSelect<T>(this IQueryable<T> query, params string[] fields) where T : class
    {
        return query.Select(SelectHelper.DynamicSelectGenerator<T>(fields));
    }
}
