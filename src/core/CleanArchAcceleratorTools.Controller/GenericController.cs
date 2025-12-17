using CleanArchAcceleratorTools.Application.Services;
using CleanArchAcceleratorTools.Controller.Models;
using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Mediator.DomainNotification.Handler;
using Microsoft.AspNetCore.Mvc;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Controller;

/// <summary>
/// Thin base API controller integrating mediator, notifications, logging, and the application service for <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">Domain entity type. Must inherit from <see cref="Entity"/>.</typeparam>
/// <typeparam name="TViewModelEntity">View model type associated with <typeparamref name="TEntity"/>.</typeparam>
/// <remarks>
/// - Publishes domain notifications via <see cref="ICleanArchMediator"/> and logs with <see cref="IApplicationLogger"/>.
/// - Aggregates notifications using <see cref="DomainNotificationEventHandler"/> and formats standardized responses.
/// - Provides helpers to return a consistent payload and to raise notifications.
/// </remarks>
public class GenericController<TEntity, TViewModelEntity> : ControllerBase where TEntity : Entity
{
    /// <summary>Mediator used to publish notifications.</summary>
    protected readonly ICleanArchMediator _mediator;

    /// <summary>Notification handler abstraction injected for mediator integration.</summary>
    protected readonly INotificationHandler<DomainNotificationEvent> _notificationHandler;

    /// <summary>Concrete notifications handler used to query and clear domain notifications.</summary>
    protected readonly DomainNotificationEventHandler _domainNotificationsHandler;

    /// <summary>Application logger for structured notifications logging.</summary>
    protected readonly IApplicationLogger _applicationLogger;

    /// <summary>Application service orchestrating operations for <typeparamref name="TEntity"/> and <typeparamref name="TViewModelEntity"/>.</summary>
    protected readonly IGenericApplicationService<TEntity, TViewModelEntity> _applicationService;

    /// <summary>
    /// Initializes a new instance of the controller.
    /// </summary>
    /// <param name="applicationService">Application service for the entity/view model.</param>
    /// <param name="mediator">Mediator for publishing notifications.</param>
    /// <param name="domainNotifications">Notification handler registered in DI.</param>
    /// <param name="applicationLogger">Application logger used for notification logs.</param>
    public GenericController(
        IGenericApplicationService<TEntity, TViewModelEntity> applicationService,
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> domainNotifications,
        IApplicationLogger applicationLogger)
    {
        _applicationService = applicationService;
        _mediator = mediator;
        _notificationHandler = domainNotifications;
        _domainNotificationsHandler = (DomainNotificationEventHandler)domainNotifications;
        _applicationLogger = applicationLogger;
    }

    #region ResponseResult

    /// <summary>
    /// Returns a standardized API response with success flag, notifications, and optional payload.
    /// </summary>
    /// <param name="result">Payload to include when the operation succeeds.</param>
    /// <param name="ignoreErrors">If true, forces success regardless of notifications.</param>
    /// <returns>An <see cref="IActionResult"/> wrapping <see cref="ResponseResult"/>.</returns>
    protected IActionResult ResponseResult(object? result = null, bool ignoreErrors = false)
    {
        var success = ignoreErrors || IsValidOperation();

        var notifications = _domainNotificationsHandler
            .GetNotifications()
            .Where(n => !n.KeepAline)
            .Select(n => n.Value)
            .ToList();

        _domainNotificationsHandler.ClearNotifications();

        return Ok(new ResponseResult
        {
            Success = success,
            Notifications = notifications,
            Data = success ? result : null
        });
    }

    /// <summary>
    /// Returns a file result when successful; otherwise a standardized API response with notifications.
    /// </summary>
    /// <param name="file">The file to return when successful.</param>
    /// <param name="ignoreErrors">If true, forces success regardless of notifications.</param>
    /// <returns><see cref="FileResult"/> when successful; otherwise an <see cref="IActionResult"/> with notifications.</returns>
    protected IActionResult ResponseResult(FileResult? file = null, bool ignoreErrors = false)
    {
        var success = ignoreErrors || IsValidOperation();

        var notifications = _domainNotificationsHandler
            .GetNotifications()
            .Where(n => !n.KeepAline)
            .Select(n => n.Value)
            .ToList();

        if (file == null || !success)
            return Ok(new ResponseResult
            {
                Success = success,
                Notifications = notifications,
                Data = null
            });

        return file;
    }

    /// <summary>
    /// Determines whether the current operation has no error notifications.
    /// </summary>
    private bool IsValidOperation()
    {
        return !_domainNotificationsHandler.HasError();
    }

    #endregion

    #region Notifications

    /// <summary>
    /// Raises a dated domain notification (prepends timestamp), logs it, and publishes it via the mediator.
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="key">Notification key/category.</param>
    /// <param name="message">Notification message.</param>
    protected async Task RaiseDatedNotificationAsync(DomainNotificationType type, string key, string message)
    {
        var timestamp = DateTime.Now.ToString(DomainMessages.DatedNotificationTimestampFormat);
        var datedMessage = DomainMessages.DatedNotificationTemplate.ToFormat(timestamp, message);

        var @event = new DomainNotificationEventBuilder(type, key)
            .WithValue(datedMessage)
            .Build();

        await LogNotificationAsync(@event.Type, @event.Value);
        await _mediator.PublishAsync(@event);
    }

    /// <summary>
    /// Raises a domain notification, logs it, and publishes it via the mediator.
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="key">Notification key/category.</param>
    /// <param name="message">Notification message.</param>
    protected async Task RaiseNotificationAsync(DomainNotificationType type, string key, string message)
    {
        var @event = new DomainNotificationEventBuilder(type, key)
            .WithValue(message)
            .Build();

        await LogNotificationAsync(@event.Type, @event.Value);
        await _mediator.PublishAsync(@event);
    }

    /// <summary>
    /// Logs a notification using the appropriate level based on <paramref name="type"/>.
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="message">Message to log.</param>
    private async Task LogNotificationAsync(DomainNotificationType type, string message)
    {
        switch (type)
        {
            case DomainNotificationType.Error:
                await _applicationLogger.LogErrorAsync(DomainMessages.LogLabelError, message);
                break;
            case DomainNotificationType.Warning:
                await _applicationLogger.LogWarningAsync(DomainMessages.LogLabelWarning, message);
                break;
            case DomainNotificationType.Information:
                await _applicationLogger.LogInformationAsync(DomainMessages.LogLabelInformation, message);
                break;
            case DomainNotificationType.Trace:
                await _applicationLogger.LogTraceAsync(DomainMessages.LogLabelTrace, message);
                break;
            case DomainNotificationType.Debug:
                await _applicationLogger.LogDebugAsync(DomainMessages.LogLabelDebug, message);
                break;
        }
    }

    #endregion
}
