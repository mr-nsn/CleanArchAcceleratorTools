using CleanArchAcceleratorTools.Domain.Constants;

namespace CleanArchAcceleratorTools.Domain.DynamicFilters;

/// <summary>
/// Attribute to mark an entity property for quick search with a specific comparison operator.
/// Used by <see cref="DynamicFilterHelper"/>; operators come from <see cref="DynamicFilterConstants"/>.
/// </summary>
public class QuickSearchAttribute : Attribute
{
    /// <summary>
    /// Comparison operator used during quick search (see <see cref="DynamicFilterConstants"/>).
    /// </summary>
    public string ComparisonOperator { get; set; }

    /// <summary>
    /// Creates the attribute with an optional operator (default: <see cref="DynamicFilterConstants.COMPARISON_OPERATOR_LIKE"/>).
    /// </summary>
    /// <param name="comparisonOperator">Operator from <see cref="DynamicFilterConstants"/>.</param>
    public QuickSearchAttribute(string comparisonOperator = DynamicFilterConstants.COMPARISON_OPERATOR_LIKE)
    {
        ComparisonOperator = comparisonOperator;
    }
}