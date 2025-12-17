using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.DynamicFilters;
using CleanArchAcceleratorTools.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace CleanArchAcceleratorTools.Domain.Models;

/// <summary>
/// Minimal base entity with identifier, creation timestamp, and validation support.
/// </summary>
/// <remarks>
/// Marks aggregate roots via <see cref="IAggregateRoot"/>. The <see cref="Id"/> participates in quick search using
/// <see cref="DynamicFilterConstants.COMPARISON_OPERATOR_EQUAL"/>.
/// </remarks>
public abstract class Entity : IAggregateRoot
{
    /// <summary>
    /// Unique identifier for the entity.
    /// </summary>
    [QuickSearch(DynamicFilterConstants.COMPARISON_OPERATOR_EQUAL)]
    public long? Id { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was created. Defaults to <see cref="DateTime.MinValue"/>.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Last validation result for this entity.
    /// </summary>
    public ValidationResult ValidationResult { get; set; }

    /// <summary>
    /// Initializes the entity with default values.
    /// </summary>
    /// <remarks>
    /// Sets <see cref="CreatedAt"/> to <see cref="DateTime.MinValue"/> and initializes <see cref="ValidationResult"/>.
    /// </remarks>
    public Entity()
    {
        ValidationResult = new ValidationResult();
    }

    /// <summary>
    /// Validates this instance using the provided validator.
    /// </summary>
    /// <typeparam name="TEntity">Entity type to validate.</typeparam>
    /// <param name="validator">Validator to apply.</param>
    /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
    public bool Validate<TEntity>(IValidator<TEntity> validator)
    {
        ValidationResult = validator.Validate((TEntity)(object)this);
        return ValidationResult.IsValid;
    }
}
