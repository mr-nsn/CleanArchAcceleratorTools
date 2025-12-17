namespace CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;

/// <summary>
/// Domain notification event for mediator publication, including identification, severity, key/value, and persistence control.
/// </summary>
public class DomainNotificationEvent : INotification
{
    /// <summary>
    /// Unique identifier for the notification.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Notification severity/classification.
    /// </summary>
    public DomainNotificationType Type { get; private set; }

    /// <summary>
    /// Key for categorizing/identifying the message.
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// Message or associated content.
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Indicates whether the notification should remain active longer.
    /// </summary>
    public bool KeepAline { get; private set; }

    /// <summary>
    /// Creates a new notification with the specified type and key.
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="key">Notification key/category.</param>
    public DomainNotificationEvent(DomainNotificationType type, string key)
    {
        Id = Guid.NewGuid();
        Type = type;
        Key = key;
        Value = string.Empty;
        DisableKeepAline();
    }

    /// <summary>
    /// Sets the notification value/message.
    /// </summary>
    /// <param name="value">Non-empty content.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null or whitespace.</exception>
    public void SetValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value), $"{nameof(value)} cannot be null or empty.");
        Value = value;
    }

    /// <summary>
    /// Enables keep-alive mode.
    /// </summary>
    public void EnableKeepAline()
    {
        KeepAline = true;
    }

    /// <summary>
    /// Disables keep-alive mode.
    /// </summary>
    public void DisableKeepAline()
    {
        KeepAline = false;
    }
}
