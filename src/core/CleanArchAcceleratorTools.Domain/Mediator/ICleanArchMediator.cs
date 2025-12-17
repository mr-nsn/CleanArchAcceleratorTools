namespace CleanArchAcceleratorTools.Domain.Mediator;

/// <summary>
/// Request contract that yields a response when handled.
/// </summary>
/// <typeparam name="TResponse">Response type produced by the handler.</typeparam>
public interface IRequest<TResponse> { }

/// <summary>
/// Handles a request and returns a response.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processes the request asynchronously.
    /// </summary>
    /// <param name="request">Request instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task producing the response.</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Marker for notifications (events) without a response.
/// </summary>
public interface INotification { }

/// <summary>
/// Handles a notification/event.
/// </summary>
/// <typeparam name="TNotification">Notification type.</typeparam>
public interface INotificationHandler<TNotification> where TNotification : INotification
{
    /// <summary>
    /// Processes the notification asynchronously.
    /// </summary>
    /// <param name="notification">Notification instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}

/// <summary>
/// Mediator for sending requests and publishing notifications to their handlers.
/// </summary>
public interface ICleanArchMediator
{
    /// <summary>
    /// Sends a request to its handler and returns the response.
    /// </summary>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <param name="request">Request to send.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task producing the handler response.</returns>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification to all registered handlers of the notification type.
    /// </summary>
    /// <typeparam name="TNotification">Notification type.</typeparam>
    /// <param name="notification">Notification to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that completes when all handlers finish processing.</returns>
    Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
}
