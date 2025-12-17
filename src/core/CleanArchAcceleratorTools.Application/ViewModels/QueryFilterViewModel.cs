namespace CleanArchAcceleratorTools.Application.ViewModels;

/// <summary>
/// Compact query options for pagination, field projection, dynamic filtering, and sorting.
/// </summary>
/// <typeparam name="TEntityViewModel">Entity/view model type being queried.</typeparam>
/// <remarks>
/// Defaults on construction:
/// - <see cref="Page"/> = 1
/// - <see cref="PageSize"/> = 10
/// Typical usage:
/// - Set <see cref="Fields"/> to project properties (dot notation supported).
/// - Configure <see cref="DynamicFilter"/> for predicate logic.
/// - Configure <see cref="DynamicSort"/> for deterministic ordering.
/// </remarks>
public class QueryFilterViewModel<TEntityViewModel>
{
    /// <summary>
    /// Current page number (1-based).
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Items per page.
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Field names to include (supports dot notation, e.g., "Customer.Address.City").
    /// </summary>
    public string[]? Fields { get; set; }

    /// <summary>
    /// Dynamic filter definition for predicate composition.
    /// </summary>
    public DynamicFilterViewModel<TEntityViewModel>? DynamicFilter { get; set; }

    /// <summary>
    /// Dynamic sort definition (e.g., "asc"/"desc").
    /// </summary>
    public DynamicSortViewModel<TEntityViewModel>? DynamicSort { get; set; }

    /// <summary>
    /// Initializes with default pagination values.
    /// </summary>
    public QueryFilterViewModel()
    {
        Page = 1;
        PageSize = 10;
    }
}