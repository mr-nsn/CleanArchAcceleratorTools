using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.DynamicFilters;
using CleanArchAcceleratorTools.Domain.Exceptions;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models.Validators;
using CleanArchAcceleratorTools.Domain.Util;
using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Domain.Models;

/// <summary>
/// Compact dynamic filter definition for building LINQ predicates.
/// </summary>
/// <typeparam name="TEntity">Entity type that derives from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Combines an optional quick search with clause groups. Use <see cref="CompileFilter"/> to produce an expression for LINQ providers.
/// </remarks>
public class DynamicFilter<TEntity> : Entity where TEntity : Entity
{
    /// <summary>
    /// Quick search term used across properties annotated with <see cref="QuickSearchAttribute"/>.
    /// </summary>
    public string? QuickSearch { get; private set; }

    /// <summary>
    /// Collection of clause groups composing the filter logic.
    /// </summary>
    public ICollection<ClauseGroup<TEntity>> ClauseGroups { get; private set; }

    /// <summary>
    /// Initializes a new filter with empty quick search and no clause groups.
    /// </summary>
    public DynamicFilter()
    {
        QuickSearch = string.Empty;
        ClauseGroups = new List<ClauseGroup<TEntity>>();
    }

    /// <summary>
    /// Sets the quick search term.
    /// </summary>
    /// <param name="search">Search value; can be null or empty.</param>
    public void SetQuickSearch(string? search)
    {
        QuickSearch = search;
    }

    /// <summary>
    /// Replaces the current clause groups.
    /// </summary>
    /// <param name="clauseGroups">New clause groups collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="clauseGroups"/> is null.</exception>
    public void SetClauseGroups(ICollection<ClauseGroup<TEntity>> clauseGroups)
    {
        ClauseGroups = clauseGroups ?? throw new ArgumentNullException(nameof(clauseGroups), DomainMessages.PropertyCannotBeNull.ToFormat($"{nameof(DynamicFilter<TEntity>)}.{nameof(ClauseGroups)}"));
    }

    /// <summary>
    /// Adds a clause group to the filter.
    /// </summary>
    /// <param name="clauseGroup">Clause group to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="clauseGroup"/> is null.</exception>
    public void AddClauseGroup(ClauseGroup<TEntity> clauseGroup)
    {
        if (clauseGroup is null) throw new ArgumentNullException(nameof(clauseGroup), DomainMessages.PropertyListCannotContainsNullValue.ToFormat($"{nameof(DynamicFilter<TEntity>)}.{nameof(ClauseGroups)}"));
        ClauseGroups.Add(clauseGroup);
    }

    /// <summary>
    /// Compiles the current filter into a predicate expression for <typeparamref name="TEntity"/>
    /// </summary>
    /// <returns>An <see cref="Expression{TDelegate}"/> suitable for LINQ Where operations.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the filter is not valid (see <see cref="IsValid"/>).</exception>
    public Expression<Func<TEntity, bool>> CompileFilter()
    {
        if (!Validate(new DynamicFilterValidator<TEntity>())) throw new DomainException(ValidationResult);
        return DynamicFilterHelper.BuildFilter(this);
    }
}

/// <summary>
/// Represents a logical group of clauses combined by a logic operator (AND/OR).
/// </summary>
/// <typeparam name="TEntity">Entity type that derives from <see cref="Entity"/>.</typeparam>
public class ClauseGroup<TEntity> where TEntity : Entity
{
    /// <summary>
    /// Logical operator used to combine clauses within the group.
    /// Expected values: <see cref="DynamicFilterConstants.LOGIC_OPERATOR_AND"/> or <see cref="DynamicFilterConstants.LOGIC_OPERATOR_OR"/>.
    /// </summary>
    public string LogicOperator { get; private set; }

    /// <summary>
    /// Collection of clauses that belong to this group.
    /// </summary>
    public ICollection<Clause<TEntity>> Clauses { get; private set; }

    /// <summary>
    /// Initializes a new clause group with default operator AND and no clauses.
    /// </summary>
    public ClauseGroup()
    {
        LogicOperator = DynamicFilterConstants.LOGIC_OPERATOR_AND;
        Clauses = new List<Clause<TEntity>>();
    }

    /// <summary>
    /// Sets the logic operator for the group.
    /// </summary>
    /// <param name="logicOperator">Logical operator, typically "&&" or "||".</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="logicOperator"/> is null.</exception>
    public void SetLogicOperator(string logicOperator)
    {
        LogicOperator = logicOperator ?? throw new ArgumentNullException(nameof(logicOperator), DomainMessages.PropertyCannotBeNull.ToFormat($"{nameof(ClauseGroup<TEntity>)}.{nameof(ClauseGroup<TEntity>.LogicOperator)}"));
    }

    /// <summary>
    /// Replaces the group's clauses.
    /// </summary>
    /// <param name="clauses">New collection of clauses.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="clauses"/> is null.</exception>
    public void SetClauses(ICollection<Clause<TEntity>> clauses)
    {
        Clauses = clauses ?? throw new ArgumentNullException(nameof(clauses), DomainMessages.PropertyCannotBeNull.ToFormat($"{nameof(ClauseGroup<TEntity>)}.{nameof(ClauseGroup<TEntity>.Clauses)}"));
    }

    /// <summary>
    /// Adds a clause to the group.
    /// </summary>
    /// <param name="clause">Clause to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="clause"/> is null.</exception>
    public void AddClause(Clause<TEntity> clause)
    {
        if (clause is null) throw new ArgumentNullException(nameof(clause), DomainMessages.PropertyListCannotContainsNullValue.ToFormat($"{nameof(ClauseGroup<TEntity>)}.{nameof(ClauseGroup<TEntity>.Clauses)}"));
        Clauses.Add(clause);
    }
}

/// <summary>
/// Represents a single comparison clause applied to a field.
/// </summary>
/// <typeparam name="TEntity">Entity type that derives from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// The <see cref="Field"/> supports nested paths using dot notation (e.g., "Customer.Address.City").
/// The <see cref="ComparisonOperator"/> should be one of <see cref="DynamicFilterConstants"/> comparison operators.
/// </remarks>
public class Clause<TEntity> where TEntity : Entity
{
    /// <summary>
    /// Logical operator to combine this clause within its group (e.g., "&&", "||").
    /// </summary>
    public string LogicOperator { get; private set; }

    /// <summary>
    /// Target field name or nested path (dot notation).
    /// </summary>
    public string Field { get; private set; }

    /// <summary>
    /// Comparison operator used against <see cref="Value"/>.
    /// </summary>
    public string ComparisonOperator { get; private set; }

    /// <summary>
    /// Value used for comparison; interpretation depends on the operator and field type.
    /// </summary>
    public string? Value { get; private set; }

    /// <summary>
    /// Initializes a new empty clause.
    /// </summary>
    public Clause()
    {
        LogicOperator = string.Empty;
        Field = string.Empty;
        ComparisonOperator = string.Empty;
        Value = string.Empty;
    }

    /// <summary>
    /// Sets the logical operator for the clause.
    /// </summary>
    /// <param name="logicOperator">Logical operator (e.g., "&&", "||").</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="logicOperator"/> is null.</exception>
    public void SetLogicOperator(string logicOperator)
    {
        LogicOperator = logicOperator ?? throw new ArgumentNullException(nameof(logicOperator), DomainMessages.PropertyCannotBeNull.ToFormat($"{nameof(Clause<TEntity>)}.{nameof(LogicOperator)}"));
    }

    /// <summary>
    /// Sets the target field name or path.
    /// </summary>
    /// <param name="field">Field name or dot-notated path.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="field"/> is null.</exception>
    public void SetField(string field)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field), DomainMessages.PropertyCannotBeNull.ToFormat($"{nameof(Clause<TEntity>)}.{nameof(Field)}"));
    }

    /// <summary>
    /// Sets the comparison operator.
    /// </summary>
    /// <param name="@operator">Operator string (e.g., "==", "!=", "&gt;", "&lt;", "&gt;=", "&lt;=", "like").</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="@operator"/> is null.</exception>
    public void SetComparisonOperator(string comparisonOperator)
    {
        ComparisonOperator = comparisonOperator ?? throw new ArgumentNullException(nameof(comparisonOperator), DomainMessages.PropertyCannotBeNull.ToFormat($"{nameof(Clause<TEntity>)}.{nameof(ComparisonOperator)}"));
    }

    /// <summary>
    /// Sets the value to be used in the comparison.
    /// </summary>
    /// <param name="value">Comparison value; can be null depending on operator semantics.</param>
    public void SetValue(string? value)
    {
        Value = value;
    }
}
