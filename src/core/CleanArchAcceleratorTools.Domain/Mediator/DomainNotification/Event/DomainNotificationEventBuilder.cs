namespace CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;

/// <summary>
/// Fluent builder to create and configure <see cref="DomainNotificationEvent"/> instances.
/// </summary>
/// <remarks>
/// Provides simple methods to set message content and keep-alive behavior, returning a ready-to-publish event.
/// </remarks>
public class DomainNotificationEventBuilder
{
    private readonly DomainNotificationEvent _domainNotificationEvent;

    /// <summary>
    /// Initializes the builder with notification type and key.
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="key">Notification key/category.</param>
    public DomainNotificationEventBuilder(DomainNotificationType type, string key)
    {
        _domainNotificationEvent = new DomainNotificationEvent(type, key);
    }

    /// <summary>
    /// Sets the notification message/value.
    /// </summary>
    /// <param name="value">Non-empty message or payload content.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null or whitespace.</exception>
    public DomainNotificationEventBuilder WithValue(string value)
    {
        _domainNotificationEvent.SetValue(value);
        return this;
    }

    /// <summary>
    /// Toggles whether the notification should remain active (keep-alive).
    /// </summary>
    /// <param name="keepAlive">If true, enables keep-alive; otherwise disables it.</param>
    /// <returns>The current builder instance.</returns>
    public DomainNotificationEventBuilder WithKeepAlive(bool keepAlive)
    {
        if (keepAlive)
            _domainNotificationEvent.EnableKeepAline();
        else
            _domainNotificationEvent.DisableKeepAline();
        return this;
    }

    /// <summary>
    /// Builds the configured <see cref="DomainNotificationEvent"/>.
    /// </summary>
    /// <returns>The constructed notification event.</returns>
    public DomainNotificationEvent Build()
    {
        return _domainNotificationEvent;
    }
}
