namespace CleanArchAcceleratorTools.Application.ViewModels;

/// <summary>
/// Lightweight paginated result view model with pagination metadata and items.
/// </summary>
/// <typeparam name="TEntityViewModel">Type of items in the result set.</typeparam>
/// <remarks>
/// Typically mapped from domain pagination results for API/UI consumption.
/// Properties are nullable to allow partial/omitted metadata.
/// </remarks>
public class PaginationResultViewModel<TEntityViewModel>
{
    /// <summary>
    /// Current 1-based page number.
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Items per page.
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Total records across all pages.
    /// </summary>
    public int? TotalRecords { get; set; }

    /// <summary>
    /// Items for the current page.
    /// </summary>
    public List<TEntityViewModel>? Result { get; set; }
}
