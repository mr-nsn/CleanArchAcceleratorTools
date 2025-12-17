using System.ComponentModel;

namespace CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;

/// <summary>
/// Defines the severity or classification of a domain notification event.
/// </summary>
/// <remarks>
/// Used by <see cref="DomainNotificationEvent"/> to categorize notifications and
/// guide handling strategies (e.g., logging level, UI prominence, or processing rules).
/// </remarks>
public enum DomainNotificationType
{
    /// <summary>
    /// Fine-grained, low-level diagnostics, typically disabled in production.
    /// </summary>
    [Description("Trace")]
    Trace = 1,

    /// <summary>
    /// Detailed information primarily useful to developers for debugging.
    /// </summary>
    [Description("Debug")]
    Debug = 2,

    /// <summary>
    /// General informational messages that highlight application progress.
    /// </summary>
    [Description("Information")]
    Information = 3,

    /// <summary>
    /// Potential issues or unexpected states that do not stop execution.
    /// </summary>
    [Description("Warning")]
    Warning = 4,

    /// <summary>
    /// Errors that prevent current operation from succeeding.
    /// </summary>
    [Description("Error")]
    Error = 5
}
