using FluentValidation;

namespace CleanArchAcceleratorTools.Domain.Models.Validators;

/// <summary>
/// Validator for <see cref="DynamicSort{TEntity}"/>, ensuring each field sort entry is valid.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
internal class DynamicSortValidator<TEntity> : AbstractValidator<DynamicSort<TEntity>> where TEntity : Entity
{
    /// <summary>
    /// Configures validation rules for the dynamic sort definition.
    /// </summary>
    public DynamicSortValidator()
    {
        FieldsOrder();
    }

    /// <summary>
    /// Adds per-item validation for <see cref="DynamicSort{TEntity}.FieldsSort"/> using <see cref="FieldSortValidator{TEntity}"/>.
    /// </summary>
    private void FieldsOrder()
    {
        RuleForEach(x => x.FieldsSort).SetValidator(new FieldSortValidator<TEntity>());
    }
}
