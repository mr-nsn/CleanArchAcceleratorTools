namespace CleanArchAcceleratorTools.Controller.Models;

/// <summary>
/// Lightweight API response wrapper (non-generic).
/// </summary>
/// <remarks>
/// Inherits from <see cref="ResponseResult{T}"/> with <see cref="object"/> as payload for endpoints without a typed model.
/// </remarks>
public class ResponseResult : ResponseResult<object>
{
}

/// <summary>
/// Lightweight API response wrapper with success flag, payload, and notifications.
/// </summary>
/// <typeparam name="T">Type of the response payload.</typeparam>
/// <remarks>
/// - <see cref="Success"/> indicates operation outcome.
/// - <see cref="Data"/> holds the payload (nullable).
/// - <see cref="Notifications"/> lists info/warning/error messages.
/// </remarks>
public class ResponseResult<T>
{
    /// <summary>
    /// Indicates whether the operation succeeded.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Response payload (nullable).
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Notifications such as information, warnings, or errors.
    /// </summary>
    public List<string> Notifications { get; set; } = new List<string>();
}
