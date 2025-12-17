namespace CleanArchAcceleratorTools.Domain.Models.Builders;

/// <summary>
/// Fluent builder for <see cref="DynamicFilter{T}"/>.
/// </summary>
/// <typeparam name="T">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Sets quick search and manages clause groups, returning a configured filter.
/// </remarks>
public class DynamicFilterBuilder<T> where T : Entity
{
    private readonly DynamicFilter<T> _dynamicFilter;

    /// <summary>
    /// Initializes the filter builder.
    /// </summary>
    public DynamicFilterBuilder()
    {
        _dynamicFilter = new DynamicFilter<T>();
    }

    /// <summary>
    /// Sets the quick search term.
    /// </summary>
    /// <param name="search">Quick search string; can be null or empty.</param>
    /// <returns>The current builder instance.</returns>
    public DynamicFilterBuilder<T> WithQuickSearch(string? search)
    {
        _dynamicFilter.SetQuickSearch(search);
        return this;
    }

    /// <summary>
    /// Replaces clause groups.
    /// </summary>
    /// <param name="clauseGroups">Clause groups to use.</param>
    /// <returns>The current builder instance.</returns>
    public DynamicFilterBuilder<T> WithClauseGroups(ICollection<ClauseGroup<T>> clauseGroups)
    {
        _dynamicFilter.SetClauseGroups(clauseGroups);
        return this;
    }

    /// <summary>
    /// Adds a clause group.
    /// </summary>
    /// <param name="clauseGroup">Clause group to add.</param>
    /// <returns>The current builder instance.</returns>
    public DynamicFilterBuilder<T> AddClauseGroup(ClauseGroup<T> clauseGroup)
    {
        _dynamicFilter.AddClauseGroup(clauseGroup);
        return this;
    }

    /// <summary>
    /// Builds the configured filter.
    /// </summary>
    /// <returns>The constructed <see cref="DynamicFilter{T}"/>.</returns>
    public DynamicFilter<T> Build()
    {
        return _dynamicFilter;
    }
}

/// <summary>
/// Fluent builder for <see cref="ClauseGroup{T}"/>.
/// </summary>
/// <typeparam name="T">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Configures logic operator and clauses, returning a ready group.
/// </remarks>
public class ClauseGroupBuilder<T> where T : Entity
{
    private readonly ClauseGroup<T> _group;

    /// <summary>
    /// Initializes the clause group builder.
    /// </summary>
    public ClauseGroupBuilder()
    {
        _group = new ClauseGroup<T>();
    }

    /// <summary>
    /// Sets the group's logic operator.
    /// </summary>
    /// <param name="logicOperator">Operator combining clauses (e.g., "&&", "||").</param>
    /// <returns>The current builder instance.</returns>
    public ClauseGroupBuilder<T> WithLogicOperator(string logicOperator)
    {
        _group.SetLogicOperator(logicOperator);
        return this;
    }

    /// <summary>
    /// Replaces the group's clauses.
    /// </summary>
    /// <param name="clauses">Collection of clauses.</param>
    /// <returns>The current builder instance.</returns>
    public ClauseGroupBuilder<T> WithClauses(ICollection<Clause<T>> clauses)
    {
        _group.SetClauses(clauses);
        return this;
    }

    /// <summary>
    /// Adds a clause to the group.
    /// </summary>
    /// <param name="clause">Clause to add.</param>
    /// <returns>The current builder instance.</returns>
    public ClauseGroupBuilder<T> AddClause(Clause<T> clause)
    {
        _group.AddClause(clause);
        return this;
    }

    /// <summary>
    /// Builds the configured group.
    /// </summary>
    /// <returns>The constructed <see cref="ClauseGroup{T}"/>.</returns>
    public ClauseGroup<T> Build()
    {
        return _group;
    }
}

/// <summary>
/// Fluent builder for <see cref="Clause{T}"/>.
/// </summary>
/// <typeparam name="T">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// Configures logic operator, field, comparison operator, and value.
/// </remarks>
public class ClauseBuilder<T> where T : Entity
{
    private readonly Clause<T> _clause;

    /// <summary>
    /// Initializes the clause builder.
    /// </summary>
    public ClauseBuilder()
    {
        _clause = new Clause<T>();
    }

    /// <summary>
    /// Sets the clause logic operator.
    /// </summary>
    /// <param name="logicOperator">Operator (e.g., "&&", "||").</param>
    /// <returns>The current builder instance.</returns>
    public ClauseBuilder<T> WithLogicOperator(string logicOperator)
    {
        _clause.SetLogicOperator(logicOperator);
        return this;
    }

    /// <summary>
    /// Sets the target field path.
    /// </summary>
    /// <param name="field">Field or dot-notated path (e.g., "Name", "Address.City").</param>
    /// <returns>The current builder instance.</returns>
    public ClauseBuilder<T> WithField(string field)
    {
        _clause.SetField(field);
        return this;
    }

    /// <summary>
    /// Sets the comparison operator.
    /// </summary>
    /// <param name="@operator">Operator (e.g., "==", "!=", "&gt;", "&lt;").</param>
    /// <returns>The current builder instance.</returns>
    public ClauseBuilder<T> WithComparisonOperator(string @operator)
    {
        _clause.SetComparisonOperator(@operator);
        return this;
    }

    /// <summary>
    /// Sets the comparison value.
    /// </summary>
    /// <param name="value">Value used in comparison; can be null.</param>
    /// <returns>The current builder instance.</returns>
    public ClauseBuilder<T> WithValue(string? value)
    {
        _clause.SetValue(value);
        return this;
    }

    /// <summary>
    /// Builds the configured clause.
    /// </summary>
    /// <returns>The constructed <see cref="Clause{T}"/>.</returns>
    public Clause<T> Build()
    {
        return _clause;
    }
}