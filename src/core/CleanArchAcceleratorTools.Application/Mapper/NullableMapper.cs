using Mapster;

namespace CleanArchAcceleratorTools.Domain.Configurations;

/// <summary>
/// Registers Mapster mappings from non-nullable primitives/common types to their nullable counterparts,
/// converting default values to <c>null</c> (and blank strings to <c>null</c>).
/// </summary>
/// <remarks>
/// Invoke <see cref="AddMapping"/> once at application startup to enable these conversions globally.
/// </remarks>
public static class NullableMapper
{
    /// <summary>
    /// Adds global Mapster configurations that map:
    /// - numeric types and <see cref="DateTime"/>: default(T) → <c>null</c>;
    /// - <see cref="string"/>: null/empty/whitespace → <c>null</c>.
    /// </summary>
    public static void AddMapping()
    {
        TypeAdapterConfig<Object, Object?>
            .NewConfig()
            .MapWith(src => src == default ? null : src);

        TypeAdapterConfig<String, String?>
            .NewConfig()
            .MapWith(src => string.IsNullOrWhiteSpace(src) ? null : src);

        TypeAdapterConfig<Int16, Int16?>
            .NewConfig()
            .MapWith(src => src == default ? null : src);

        TypeAdapterConfig<Int32, Int32?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<Int64, Int64?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<Byte, Byte?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<Decimal, Decimal?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig <Double, Double?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<Single, Single?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<UInt16, UInt16?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<UInt32, UInt32?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<UInt64, UInt64?>
            .NewConfig()
            .MapWith(src => src  == default ? null : src);

        TypeAdapterConfig<DateTime, DateTime?>
            .NewConfig()
            .MapWith(src => src == default ? null : src);
    }
}
