using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Controller.Models;

/// <summary>
/// Extensions to convert a non-generic <see cref="ResponseResult"/> into a typed <see cref="ResponseResult{T}"/>.
/// </summary>
public static class ResponseResultExtensions
{
    /// <summary>
    /// Creates a typed response by copying <see cref="ResponseResult{T}.Success"/> and <see cref="ResponseResult{T}.Notifications"/>
    /// and casting the payload to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Target payload type.</typeparam>
    /// <param name="source">The non-generic response to convert.</param>
    /// <returns>A new <see cref="ResponseResult{T}"/> with the same success flag and notifications, and a typed payload.</returns>
    /// <exception cref="InvalidCastException">Thrown when <c>source.Data</c> cannot be cast to <typeparamref name="T"/>.</exception>
    public static ResponseResult<T> ToTyped<T>(this ResponseResult source)
    {
        var dataType = source.Data?.GetType().Name ?? "null";
        var targetType = typeof(T).Name;
        return new ResponseResult<T>
        {
            Success = source.Success,
            Data = source.Data is T t ? t : throw new InvalidCastException(DomainMessages.UnableToCastTypeToTargetType.ToFormat(dataType, targetType)),
            Notifications = source.Notifications?.ToList() ?? new List<string>()
        };
    }
}
