using FluentValidation;

namespace CleanArchAcceleratorTools.Domain.Models.Validators;

/// <summary>
/// Validator for <see cref="DynamicFilter{TEntity}"/>, ensuring all clause groups are valid.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
internal class DynamicFilterValidator<TEntity> : AbstractValidator<DynamicFilter<TEntity>> where TEntity : Entity
{
    /// <summary>
    /// Configures validation for the filter's clause groups.
    /// </summary>
    public DynamicFilterValidator()
    {
        ClauseGroups();
    }

    /// <summary>
    /// Adds per-item validation for <see cref="DynamicFilter{TEntity}.ClauseGroups"/> using <see cref="ClauseGroupValidator{TEntity}"/>.
    /// </summary>
    private void ClauseGroups()
    {
        RuleForEach(x => x.ClauseGroups).SetValidator(new ClauseGroupValidator<TEntity>());
    }
}
