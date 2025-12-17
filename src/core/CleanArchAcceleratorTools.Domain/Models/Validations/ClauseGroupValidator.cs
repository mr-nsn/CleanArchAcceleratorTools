using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;
using FluentValidation;

namespace CleanArchAcceleratorTools.Domain.Models.Validators;

/// <summary>
/// Validator for <see cref="ClauseGroup{TEntity}"/> ensuring a valid logic operator (AND/OR)
/// and validating each contained clause.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
internal class ClauseGroupValidator<TEntity> : AbstractValidator<ClauseGroup<TEntity>> where TEntity : Entity
{
    /// <summary>
    /// Configures validation rules for logic operator and clauses.
    /// </summary>
    public ClauseGroupValidator()
    {
        LogicOperator();
        Clauses();
    }

    /// <summary>
    /// Adds rules for <see cref="ClauseGroup{TEntity}.LogicOperator"/>:
    /// - Not empty.
    /// - Must be either <see cref="DynamicFilterConstants.LOGIC_OPERATOR_AND"/> or <see cref="DynamicFilterConstants.LOGIC_OPERATOR_OR"/>.
    /// </summary>
    private void LogicOperator()
    {
        RuleFor(x => x.LogicOperator)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(ClauseGroup<TEntity>)}.{nameof(ClauseGroup<TEntity>.LogicOperator)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });

        When(x => !string.IsNullOrEmpty(x.LogicOperator), () =>
        {
            RuleFor(x => x.LogicOperator)
                .Must(BeAValidLogicOperator)
                .WithMessage(x =>
                {
                    var value = x.LogicOperator;
                    var property = $"{nameof(ClauseGroup<TEntity>)}.{nameof(ClauseGroup<TEntity>.LogicOperator)}";
                    var allowedValues = string.Join(", ", new[] { DynamicFilterConstants.LOGIC_OPERATOR_AND, DynamicFilterConstants.LOGIC_OPERATOR_OR });
                    return DomainMessages.InvalidPropertyAssignWithAllowed.ToFormat(value, property, allowedValues);
                });
        });
    }

    /// <summary>
    /// Adds per-item validation for <see cref="ClauseGroup{TEntity}.Clauses"/> using <see cref="ClauseValidator{TEntity}"/>.
    /// </summary>
    private void Clauses()
    {
        RuleForEach(x => x.Clauses).SetValidator(new ClauseValidator<TEntity>());
    }

    #region Auxiliary Methods

    /// <summary>
    /// Checks whether the provided operator is a valid logic operator for clause groups.
    /// </summary>
    /// <param name="logicOperator">Operator string to validate.</param>
    /// <returns><c>true</c> if operator is AND or OR; otherwise, <c>false</c>.</returns>
    private bool BeAValidLogicOperator(string logicOperator)
    {
        switch (logicOperator)
        {
            case DynamicFilterConstants.LOGIC_OPERATOR_AND:
                return true;
            case DynamicFilterConstants.LOGIC_OPERATOR_OR:
                return true;
            default:
                return false;
        }
    }

    #endregion
}
