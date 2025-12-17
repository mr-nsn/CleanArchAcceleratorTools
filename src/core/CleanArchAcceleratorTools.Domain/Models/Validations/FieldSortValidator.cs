using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;
using FluentValidation;

namespace CleanArchAcceleratorTools.Domain.Models.Validators;

/// <summary>
/// Validator for <see cref="FieldSort{TEntity}"/> ensuring a non-empty field and a valid sort order ("asc"/"desc").
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
internal class FieldSortValidator<TEntity> : AbstractValidator<FieldSort<TEntity>> where TEntity : Entity
{
    /// <summary>
    /// Configures validation rules for field name and sort order.
    /// </summary>
    public FieldSortValidator()
    {
        Field();
        Order();
    }

    /// <summary>
    /// Adds rules for <see cref="FieldSort{TEntity}.Field"/>:
    /// - Not empty.
    /// </summary>
    private void Field()
    {
        RuleFor(x => x.Field)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(FieldSort<TEntity>)}.{nameof(FieldSort<TEntity>.Field)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });
    }

    /// <summary>
    /// Adds rules for <see cref="FieldSort{TEntity}.Order"/>:
    /// - Not empty.
    /// - Must be <see cref="DynamicSortConstants.SORT_ORDER_ASC"/> or <see cref="DynamicSortConstants.SORT_ORDER_DESC"/>.
    /// </summary>
    private void Order()
    {
        RuleFor(x => x.Order)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(FieldSort<TEntity>)}.{nameof(FieldSort<TEntity>.Order)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });

        When(x => !string.IsNullOrEmpty(x.Order), () =>
        {
            RuleFor(x => x.Order)
                .Must(BeAValidOrder)
                .WithMessage(x =>
                {
                    var value = x.Order;
                    var property = $"{nameof(FieldSort<TEntity>)}.{nameof(FieldSort<TEntity>.Order)}";
                    var allowedValues = string.Join(", ", DynamicSortConstants.ValidSortOrders);
                    return DomainMessages.InvalidPropertyAssignWithAllowed.ToFormat(value, property, allowedValues);
                });
        });
    }

    #region Auxiliary Methods

    /// <summary>
    /// Validates the sort order value.
    /// </summary>
    /// <param name="order">Sort direction string to validate.</param>
    /// <returns><c>true</c> if the order is "asc" or "desc"; otherwise, <c>false</c>.</returns>
    private bool BeAValidOrder(string order)
    {
        return DynamicSortConstants.ValidSortOrders.Contains(order);
    }

    #endregion
}
