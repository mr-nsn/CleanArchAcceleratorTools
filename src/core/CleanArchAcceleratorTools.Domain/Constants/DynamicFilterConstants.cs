namespace CleanArchAcceleratorTools.Domain.Constants;

/// <summary>
/// Provides constant string values for logical and comparison operators
/// used by dynamic filtering components across the domain layer.
/// </summary>
/// <remarks>
/// These constants standardize operator representations to avoid magic strings
/// and ensure consistency across filtering, querying, and expression-building features.
/// </remarks>
public static class DynamicFilterConstants
{
    /// <summary>
    /// Logical AND operator: <c>&amp;&amp;</c>
    /// </summary>
    public const string LOGIC_OPERATOR_AND = "&&";

    /// <summary>
    /// Logical OR operator: <c>||</c>
    /// </summary>
    public const string LOGIC_OPERATOR_OR = "||";

    /// <summary>
    /// Equality comparison operator: <c>==</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_EQUAL = "==";

    /// <summary>
    /// Inequality comparison operator: <c>!=</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_NOT_EQUAL = "!=";

    /// <summary>
    /// Greater-than comparison operator: <c>&gt;</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_GREATER_THAN = ">";

    /// <summary>
    /// Greater-than-or-equal comparison operator: <c>&gt;=</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_GREATER_THAN_OR_EQUAL = ">=";

    /// <summary>
    /// Less-than comparison operator: <c>&lt;</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_LESS_THAN = "<";

    /// <summary>
    /// Less-than-or-equal comparison operator: <c>&lt;=</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_LESS_THAN_OR_EQUAL = "<=";

    /// <summary>
    /// Pattern-matching operator (commonly case-insensitive; implementation-dependent): <c>like</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_LIKE = "like";

    /// <summary>
    /// Negative pattern-matching operator: <c>not_like</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_NOT_LIKE = "not_like";

    /// <summary>
    /// String starts-with operator (commonly case-insensitive; implementation-dependent): <c>starts_with</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_STARTS_WITH = "starts_with";

    /// <summary>
    /// Negative string starts-with operator: <c>not_starts_with</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_NOT_STARTS_WITH = "not_starts_with";

    /// <summary>
    /// String ends-with operator (commonly case-insensitive; implementation-dependent): <c>ends_with</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_ENDS_WITH = "ends_with";

    /// <summary>
    /// Negative string ends-with operator: <c>not_ends_with</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_NOT_ENDS_WITH = "not_ends_with";

    /// <summary>
    /// Checks if value is empty (e.g., null, empty string, or collection with zero items; implementation-dependent): <c>is_empty</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_IS_EMPTY = "is_empty";

    /// <summary>
    /// Checks if value is not empty: <c>is_not_empty</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_IS_NOT_EMPTY = "is_not_empty";

    /// <summary>
    /// Membership operator (value is within a set): <c>in</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_IN = "in";

    /// <summary>
    /// Negative membership operator (value is not within a set): <c>not_in</c>
    /// </summary>
    public const string COMPARISON_OPERATOR_NOT_IN = "not_in";

    public static readonly string[] ValidLogicOperators = new[]
    {
        LOGIC_OPERATOR_AND,
        LOGIC_OPERATOR_OR
    };

    public static readonly string[] ValidComparisonOperators = new[]
    {
        COMPARISON_OPERATOR_EQUAL,
        COMPARISON_OPERATOR_NOT_EQUAL,
        COMPARISON_OPERATOR_GREATER_THAN,
        COMPARISON_OPERATOR_GREATER_THAN_OR_EQUAL,
        COMPARISON_OPERATOR_LESS_THAN,
        COMPARISON_OPERATOR_LESS_THAN_OR_EQUAL,
        COMPARISON_OPERATOR_LIKE,
        COMPARISON_OPERATOR_NOT_LIKE,
        COMPARISON_OPERATOR_STARTS_WITH,
        COMPARISON_OPERATOR_NOT_STARTS_WITH,
        COMPARISON_OPERATOR_ENDS_WITH,
        COMPARISON_OPERATOR_NOT_ENDS_WITH,
        COMPARISON_OPERATOR_IS_EMPTY,
        COMPARISON_OPERATOR_IS_NOT_EMPTY,
        COMPARISON_OPERATOR_IN,
        COMPARISON_OPERATOR_NOT_IN
    };
}
