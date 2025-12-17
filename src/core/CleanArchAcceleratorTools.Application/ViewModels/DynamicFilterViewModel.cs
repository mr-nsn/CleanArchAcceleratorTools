namespace CleanArchAcceleratorTools.Application.ViewModels;

/// <summary>
/// Lightweight dynamic filter for queries over <typeparamref name="TEntityViewModel"/>.
/// </summary>
/// <typeparam name="TEntityViewModel">Entity type being filtered.</typeparam>
/// <remarks>
/// Holds an optional quick search and logical clause groups; typically mapped to domain filters and compiled to LINQ.
/// </remarks>
public class DynamicFilterViewModel<TEntityViewModel>
{
    /// <summary>
    /// Optional quick search text applied to designated properties.
    /// </summary>
    public string? QuickSearch { get; set; }

    /// <summary>
    /// Optional logical clause groups composing the filter.
    /// </summary>
    public ICollection<ClauseGroupViewModel<TEntityViewModel>>? ClauseGroups { get; set; }
}

/// <summary>
/// Logical group of clauses combined with a logic operator (AND/OR).
/// </summary>
/// <typeparam name="TEntityViewModel">Entity type being filtered.</typeparam>
public class ClauseGroupViewModel<TEntityViewModel>
{
    /// <summary>
    /// Operator combining clauses in this group. Expected: "&&" or "||".
    /// </summary>
    public string? LogicOperator { get; set; }

    /// <summary>
    /// Clauses contained in this group.
    /// </summary>
    public ICollection<ClauseViewModel<TEntityViewModel>>? Clauses { get; set; }
}

/// <summary>
/// Single comparison clause targeting a field.
/// </summary>
/// <typeparam name="TEntityViewModel">Entity type being filtered.</typeparam>
/// <remarks>
/// - <see cref="Field"/> supports dot notation (e.g., "Customer.Address.City").
/// - <see cref="ComparisonOperator"/> should be a supported operator (e.g., "==", "!=", "&gt;", "&lt;", "&gt;=", "&lt;=", "like").
/// </remarks>
public class ClauseViewModel<TEntityViewModel>
{
    /// <summary>
    /// Operator to combine this clause within its group. Expected: "&&" or "||".
    /// </summary>
    public string? LogicOperator { get; set; }

    /// <summary>
    /// Target field name or nested path.
    /// </summary>
    public string? Field { get; set; }

    /// <summary>
    /// Operator used against <see cref="Value"/>.
    /// </summary>
    public string? ComparisonOperator { get; set; }

    /// <summary>
    /// Value used for comparison; semantics depend on operator and field type.
    /// </summary>
    public object? Value { get; set; }
}
