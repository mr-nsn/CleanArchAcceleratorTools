namespace CleanArchAcceleratorTools.Application.ViewModels;

/// <summary>
/// Lightweight dynamic sort options for queries over <typeparamref name="TEntityViewModel"/>.
/// </summary>
/// <typeparam name="TEntityViewModel">The entity type being sorted.</typeparam>
/// <remarks>
/// Holds field orders (field path + direction) to be mapped to domain sort definitions and compiled into LINQ order expressions.
/// </remarks>
public class DynamicSortViewModel<TEntityViewModel>
{
    /// <summary>
    /// Collection of field order definitions (field and direction).
    /// </summary>
    public ICollection<FieldSortViewModel<TEntityViewModel>>? FieldsOrder { get; set; }
}

/// <summary>
/// Single field sort order for <typeparamref name="TEntityViewModel"/>.
/// </summary>
/// <typeparam name="TEntityViewModel">The entity type being sorted.</typeparam>
/// <remarks>
/// - <see cref="Field"/> supports dot notation (e.g., "Customer.Address.City").
/// - <see cref="Order"/> should be "asc" or "desc" (case-insensitive).
/// </remarks>
public class FieldSortViewModel<TEntityViewModel>
{
    /// <summary>
    /// Target field name or nested path (dot notation).
    /// </summary>
    public string? Field { get; set; }

    /// <summary>
    /// Sort direction, typically "asc" or "desc".
    /// </summary>
    public string? Order { get; set; }
}