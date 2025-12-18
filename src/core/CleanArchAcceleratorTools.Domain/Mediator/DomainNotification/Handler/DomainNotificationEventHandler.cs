using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;

namespace CleanArchAcceleratorTools.Mediator.DomainNotification.Handler;

/// <summary>
/// Lightweight handler that stores and manages <see cref="DomainNotificationEvent"/> instances in memory.
/// </summary>
/// <remarks>
/// Supports adding, querying, and clearing notifications, including type-based filters and keep-alive retention.
/// Designed for mediator pipelines via <see cref="INotificationHandler{TNotification}"/>.
/// </remarks>
public class DomainNotificationEventHandler : INotificationHandler<DomainNotificationEvent>
{
    /// <summary>
    /// Internal storage for notifications.
    /// </summary>
    private List<DomainNotificationEvent> _notifications;

    /// <summary>
    /// Initializes the handler.
    /// </summary>
    public DomainNotificationEventHandler()
    {
        _notifications ??= new List<DomainNotificationEvent>();
    }

    /// <summary>
    /// Stores an incoming notification.
    /// </summary>
    /// <param name="notification">Notification to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A completed task.</returns>
    public async Task HandleAsync(DomainNotificationEvent notification, CancellationToken cancellationToken)
    {
        _notifications.Add(notification);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Checks if there are any stored notifications.
    /// </summary>
    /// <returns><c>true</c> if any exist; otherwise, <c>false</c>.</returns>
    public bool HasNotifications()
    {
        return GetNotifications().Any();
    }

    /// <summary>
    /// Retrieves notifications, optionally filtered by type.
    /// </summary>
    /// <param name="type">Optional type filter.</param>
    /// <returns>List of matching notifications.</returns>
    public List<DomainNotificationEvent> GetNotifications(DomainNotificationType? type = null)
    {
        return _notifications.Where(x => type is null || x.Type == type).ToList();
    }

    /// <summary>
    /// Retrieves notifications, filtered by key.
    /// </summary>
    /// <param name="key">Key to filter notifications.</param>
    /// <returns>List of matching notifications.</returns>
    public List<DomainNotificationEvent> GetNotifications(string key)
    {
        return _notifications.Where(x => x.Key == key).ToList();
    }

    /// <summary>
    /// Checks if any error notifications exist.
    /// </summary>
    /// <returns><c>true</c> if errors exist; otherwise, <c>false</c>.</returns>
    public bool HasError()
    {
        return GetErrors() != null ? GetErrors().Any() : false;
    }

    /// <summary>
    /// Gets notifications of type Error.
    /// </summary>
    /// <returns>List of error notifications.</returns>
    public virtual List<DomainNotificationEvent> GetErrors()
    {
        return GetNotifications(DomainNotificationType.Error);
    }      

    /// <summary>
    /// Clears notifications, keeping only those marked as keep-alive.
    /// </summary>
    public void ClearNotifications()
    {
        _notifications = _notifications.Where(n => n.KeepAline).ToList();
    }

    /// <summary>
    /// Removes notifications matching the specified key.
    /// </summary>
    /// <param name="key">Notification key to remove.</param>
    public void ClearNotifications(string key)
    {
        _notifications = _notifications.Where(n => n.Key != key).ToList();
    }
}
