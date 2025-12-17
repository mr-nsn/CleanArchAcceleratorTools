namespace CleanArchAcceleratorTools.Domain.Util;

public static class StringExtensions
{
    public static string ToFormat(this string? value, params object?[] args)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        if (args is null || args.Length == 0)
            return value;

        return string.Format(value, args);
    }
}
