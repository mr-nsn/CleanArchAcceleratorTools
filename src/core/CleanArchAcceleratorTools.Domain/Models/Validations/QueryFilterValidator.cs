using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;
using FluentValidation;

namespace CleanArchAcceleratorTools.Domain.Models.Validators;

/// <summary>
/// Validator for <see cref="QueryFilter{TEntity}"/> ensuring valid pagination, field selection, and nested filter/sort definitions.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
internal class QueryFilterValidator<TEntity> : AbstractValidator<QueryFilter<TEntity>> where TEntity : Entity
{
    /// <summary>
    /// Configures validation rules for page, page size, fields, dynamic filter, and sort.
    /// </summary>
    public QueryFilterValidator()
    {
        Page();
        PageSize();
        Fields();
        DynamicFilter();
        DynamicSort();
    }

    /// <summary>
    /// Adds rules for <see cref="QueryFilter{TEntity}.Page"/>:
    /// - Not empty.
    /// - Greater than or equal to 1.
    /// </summary>
    private void Page()
    {
        RuleFor(x => x.Page)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(QueryFilter<TEntity>)}.{nameof(QueryFilter<TEntity>.Page)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });

        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x =>
            {
                var value = x.Page;
                var property = $"{nameof(QueryFilter<TEntity>)}.{nameof(QueryFilter<TEntity>.Page)}";
                return DomainMessages.ValueMustBeGreaterThanOrEqualTo.ToFormat(property, 1);
            });
    }

    /// <summary>
    /// Adds rules for <see cref="QueryFilter{TEntity}.PageSize"/>:
    /// - Not empty.
    /// - Greater than or equal to 1.
    /// </summary>
    private void PageSize()
    {
        RuleFor(x => x.PageSize)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(QueryFilter<TEntity>)}.{nameof(QueryFilter<TEntity>.PageSize)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            });

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage(x =>
            {
                var value = x.PageSize;
                var property = $"{nameof(QueryFilter<TEntity>)}.{nameof(QueryFilter<TEntity>.PageSize)}";
                return DomainMessages.ValueMustBeGreaterThanOrEqualTo.ToFormat(property, 1);
            });
    }

    /// <summary>
    /// Adds rules for <see cref="QueryFilter{TEntity}.Fields"/>:
    /// - Not empty.
    /// </summary>
    private void Fields()
    {
        RuleFor(x => x.Fields)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(QueryFilter<TEntity>)}.{nameof(QueryFilter<TEntity>.Fields)}";
                return DomainMessages.PropertyCannotBeEmpty.ToFormat(property);
            }); 
    }

    /// <summary>
    /// Adds nested validation for <see cref="QueryFilter{TEntity}.DynamicFilter"/> using <see cref="DynamicFilterValidator{TEntity}"/>.
    /// </summary>
    private void DynamicFilter()
    {
        RuleFor(x => x.DynamicFilter).SetValidator(new DynamicFilterValidator<TEntity>());
    }

    /// <summary>
    /// Adds nested validation for <see cref="QueryFilter{TEntity}.DynamicSort"/> using <see cref="DynamicSortValidator{TEntity}"/>.
    /// </summary>
    private void DynamicSort()
    {
        RuleFor(x => x.DynamicSort).SetValidator(new DynamicSortValidator<TEntity>());
    }
}
