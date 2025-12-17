using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;
using FluentValidation;

namespace CleanArchAcceleratorTools.Domain.Models.Validators;

/// <summary>
/// Validator for <see cref="Clause{TEntity}"/> ensuring a valid logic operator, field, comparison operator, and value.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
internal class ClauseValidator<TEntity> : AbstractValidator<Clause<TEntity>> where TEntity : Entity
{
    /// <summary>
    /// Configures validation rules for logic operator, field, comparison operator, and value.
    /// </summary>
    public ClauseValidator()
    {
        LogicOperator();
        Field();
        ComparisonOperator();
        Value();
    }
    
    /// <summary>
    /// Adds rules for <see cref="Clause{TEntity}.LogicOperator"/>:
    /// - Not empty.
    /// - Must be either <see cref="DynamicFilterConstants.LOGIC_OPERATOR_AND"/> or <see cref="DynamicFilterConstants.LOGIC_OPERATOR_OR"/>.
    /// </summary>
    private void LogicOperator()
    {
        RuleFor(x => x.LogicOperator)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(Clause<TEntity>)}.{nameof(Clause<TEntity>.LogicOperator)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });

        When(x => !string.IsNullOrEmpty(x.LogicOperator), () =>
        {
            RuleFor(x => x.LogicOperator)
                .Must(BeAValidLogicOperator)
                .WithMessage(x =>
                {
                    var value = x.LogicOperator;
                    var property = $"{nameof(Clause<TEntity>)}.{nameof(Clause<TEntity>.LogicOperator)}";
                    var allowedValues = string.Join(", ", DynamicFilterConstants.ValidLogicOperators);
                    return DomainMessages.InvalidPropertyAssignWithAllowed.ToFormat(value, property, allowedValues);
                });
        });
    }

    /// <summary>
    /// Adds rules for <see cref="Clause{TEntity}.Field"/>:
    /// - Not empty.
    /// </summary>
    private void Field()
    {
        RuleFor(x => x.Field)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(Clause<TEntity>)}.{nameof(Clause<TEntity>.Field)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });
    }

    /// <summary>
    /// Adds rules for <see cref="Clause{TEntity}.ComparisonOperator"/>:
    /// - Not empty.
    /// - Must be one of the supported comparison operators in <see cref="DynamicFilterConstants"/>.
    /// </summary>
    private void ComparisonOperator()
    {
        RuleFor(x => x.ComparisonOperator)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(Clause<TEntity>)}.{nameof(Clause<TEntity>.ComparisonOperator)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });

        When(x => !string.IsNullOrEmpty(x.ComparisonOperator), () =>
        {
            RuleFor(x => x.ComparisonOperator)
                .Must(BeAValidComparisonOperator)
                .WithMessage(x =>
                {
                    var value = x.ComparisonOperator;
                    var property = $"{nameof(Clause<TEntity>)}.{nameof(Clause<TEntity>.ComparisonOperator)}";
                    var allowedValues = string.Join(", ", DynamicFilterConstants.ValidComparisonOperators);
                    return DomainMessages.InvalidPropertyAssignWithAllowed.ToFormat(value, property, allowedValues);
                });
        });
    }

    /// <summary>
    /// Adds rules for <see cref="Clause{TEntity}.Value"/>:
    /// - Not empty.
    /// </summary>
    private void Value()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(Clause<TEntity>)}.{nameof(Clause<TEntity>.Value)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });
    }

    #region Auxiliary Methods

    /// <summary>
    /// Validates the logic operator for clause combination.
    /// </summary>
    /// <param name="comparisonOperator">Operator string to validate.</param>
    /// <returns><c>true</c> if AND or OR; otherwise, <c>false</c>.</returns>
    private bool BeAValidLogicOperator(string logicOperator)
    {
        return DynamicFilterConstants.ValidLogicOperators.Contains(logicOperator);
    }

    /// <summary>
    /// Validates that the comparison operator is one of the supported constants.
    /// </summary>
    /// <param name="comparisonOperator">Operator string to validate.</param>
    /// <returns><c>true</c> if supported; otherwise, <c>false</c>.</returns>
    private bool BeAValidComparisonOperator(string comparisonOperator)
    {
        return DynamicFilterConstants.ValidComparisonOperators.Contains(comparisonOperator);
    }

    #endregion
}
