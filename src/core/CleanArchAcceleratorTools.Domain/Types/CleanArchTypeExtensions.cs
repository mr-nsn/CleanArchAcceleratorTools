namespace CleanArchAcceleratorTools.Domain.Types;

/// <summary>
/// Helper methods for classifying and inspecting CLR types used across dynamic filters, selects, and sorting.
/// </summary>
/// <remarks>
/// Definitions:
/// - Simple: primitives, enums, strings, decimals, <see cref="DateTime"/>, and nullable versions of these.
/// - Complex: not simple and not a collection.
/// - Collection: types under <c>System.Collections.Generic</c> (excluding <see cref="string"/>).
/// </remarks>
public static class CleanArchTypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is considered simple.
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <returns><c>true</c> for primitives, enums, <see cref="string"/>, <see cref="decimal"/>, <see cref="DateTime"/>, or their nullable variants; otherwise <c>false</c>.</returns>
    public static bool IsSimple(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            // Nullable type, check if the nested type is simple.
            return IsSimple(type.GetGenericArguments()[0]);
        }

        return type.IsPrimitive
          || type.IsEnum
          || type.Equals(typeof(string))
          || type.Equals(typeof(decimal))
          || type.Equals(typeof(DateTime));
    }

    /// <summary>
    /// Determines whether the specified type is considered complex.
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <returns><c>true</c> when the type is neither simple nor a collection; otherwise <c>false</c>.</returns>
    public static bool IsComplex(Type type)
    {
        return !IsSimple(type) && !IsCollection(type);
    }

    /// <summary>
    /// Determines whether the specified type is a collection (excluding <see cref="string"/>).
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <returns><c>true</c> if the type resides under <c>System.Collections.Generic</c> and is not <see cref="string"/>; otherwise <c>false</c>.</returns>
    public static bool IsCollection(Type type)
    {
        var typeFullName = type.FullName ?? string.Empty;
        var collectionsNameSpace = string.Format("{0}.{1}.{2}", nameof(System), nameof(System.Collections), nameof(System.Collections.Generic));
        return typeFullName.StartsWith(collectionsNameSpace) && type != typeof(string);
    }

    /// <summary>
    /// Gets the base type for enums (underlying numeric type) and nullable types (their inner type); otherwise returns the input type.
    /// </summary>
    /// <param name="type">Input type.</param>
    /// <returns>Underlying enum or nullable type; the original type when not applicable; or <c>null</c> when <paramref name="type"/> is null.</returns>
    public static Type? GetBaseType(this Type type)
    {
        if (type == null) return null;

        if (type.IsEnum)
            return Enum.GetUnderlyingType(type);

        return type.IsNullableType() ? Nullable.GetUnderlyingType(type) : type;
    }

    /// <summary>
    /// Indicates whether the specified type is a <see cref="Nullable{T}"/> generic type.
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <returns><c>true</c> if the type is <see cref="Nullable{T}"/>; otherwise <c>false</c>.</returns>
    public static bool IsNullableType(this Type type)
    {
        return type.IsGenericType(typeof(Nullable<>));
    }

    /// <summary>
    /// Indicates whether the specified type is a generic type of the provided generic type definition.
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <param name="genericType">Generic type definition, e.g., <see cref="Nullable{T}"/>.</param>
    /// <returns><c>true</c> if <paramref name="type"/> is generic and matches <paramref name="genericType"/>; otherwise <c>false</c>.</returns>
    public static bool IsGenericType(this Type type, Type genericType)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == genericType;
    }

    /// <summary>
    /// Indicates whether the specified type is a numeric CLR type.
    /// </summary>
    /// <param name="type">Type to check.</param>
    /// <returns><c>true</c> for standard numeric types; otherwise <c>false</c>.</returns>
    /// <remarks>
    /// Considered numeric: <see cref="byte"/>, <see cref="decimal"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="long"/>,
    /// <see cref="sbyte"/>, <see cref="float"/>, <see cref="ushort"/>, <see cref="uint"/>, <see cref="ulong"/>.
    /// </remarks>
    public static bool IsNumericType(this Type type)
    {
        if (type == null)
            return false;

        return new[]
        {
            typeof(Byte),
            typeof(Decimal),
            typeof(Double),
            typeof(Int16),
            typeof(Int32),
            typeof(Int64),
            typeof(SByte),
            typeof(Single),
            typeof(UInt16),
            typeof(UInt32),
            typeof(UInt64)
        }.Contains(type);
    }

    public static Type MakeNullable(this Type t)
    {
        // Check if the type is already nullable or a reference type
        if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return t;
        }
        if (!t.IsValueType)
        {
            // Reference types (like string, object, custom classes) are 
            // naturally nullable at runtime in C# versions prior to 8.0,
            // or have compiler-time NRT annotations in C# 8.0+.
            return t;
        }

        // Create the Nullable<T> type using reflection
        return typeof(Nullable<>).MakeGenericType(t);
    }
}
