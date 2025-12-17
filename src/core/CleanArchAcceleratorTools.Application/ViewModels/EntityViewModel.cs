using FluentValidation;

namespace CleanArchAcceleratorTools.Application.ViewModels;

/// <summary>
/// Minimal base view model with identifier and creation timestamp for application-layer DTOs.
/// </summary>
/// <remarks>
/// Properties are nullable to support partial payloads and mapping scenarios.
/// </remarks>
public abstract class EntityViewModel
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was created.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
}
