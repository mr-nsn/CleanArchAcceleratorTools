namespace CleanArchAcceleratorTools.Domain.Constants;

/// <summary>
/// Provides constant string values representing sort order directions
/// used by dynamic sorting features across the domain layer.
/// </summary>
/// <remarks>
/// These constants standardize sort order representations to avoid magic strings
/// and ensure consistency across sorting, querying, and expression-building components.
/// </remarks>
public static class DynamicSortConstants
{
    /// <summary>
    /// Ascending sort order: <c>asc</c>.
    /// </summary>
    public const string SORT_ORDER_ASC = "asc";

    /// <summary>
    /// Descending sort order: <c>desc</c>.
    /// </summary>
    public const string SORT_ORDER_DESC = "desc";

    
    public static readonly string[] ValidSortOrders = new[] { SORT_ORDER_ASC, SORT_ORDER_DESC };
}
