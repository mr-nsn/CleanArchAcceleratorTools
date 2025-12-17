using System.Collections.Concurrent;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Domain.Mediator;

/// <summary>
/// Lightweight mediator implementation that dispatches requests and notifications to handlers resolved via <see cref="IServiceProvider"/>.
/// </summary>
/// <remarks>
/// Caches handlers for performance and uses reflection to invoke <c>HandleAsync</c>. Thread-safe via <see cref="ConcurrentDictionary{TKey, TValue}"/>.
/// </remarks>
public class CleanArchMediator : ICleanArchMediator
{
    /// <summary>
    /// Service provider used to resolve request and notification handlers.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Cache of request handler instances keyed by their constructed handler type
    /// (<c>IRequestHandler&lt;TRequest, TResponse&gt;</c>).
    /// </summary>
    private readonly ConcurrentDictionary<Type, object?> _requestHandlerCache = new();

    /// <summary>
    /// Cache of notification handler instances keyed by notification type
    /// (<c>INotificationHandler&lt;TNotification&gt;</c>).
    /// </summary>
    private readonly ConcurrentDictionary<Type, IEnumerable<object>> _notificationHandlersCache = new();

    /// <summary>
    /// Creates a mediator that resolves handlers from the provided service provider.
    /// </summary>
    /// <param name="serviceProvider">Service provider to resolve handlers.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serviceProvider"/> is null.</exception>
    public CleanArchMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc cref="ICleanArchMediator.SendAsync{TResponse}(IRequest{TResponse}, CancellationToken)"/>
    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

        var handlerObj = _requestHandlerCache.GetOrAdd(handlerType, _ => _serviceProvider.GetService(handlerType));

        if (handlerObj is null)
        {
            throw new InvalidOperationException(DomainMessages.NoHandlerRegisteredExpectedService.ToFormat(handlerType.FullName, requestType.FullName));
        }

        var handleMethod = handlerObj.GetType().GetMethod("HandleAsync");
        if (handleMethod is null)
        {
            throw new InvalidOperationException(DomainMessages.TypeDoesNotImplementHandleAsync.ToFormat(handlerObj.GetType().FullName));
        }

        var taskObj = handleMethod.Invoke(handlerObj, new object?[] { request, cancellationToken }) as Task<TResponse>;
        if (taskObj is null)
        {
            throw new InvalidOperationException(DomainMessages.HandleAsyncReturnedNullOrInvalidTask.ToFormat(handlerObj.GetType().FullName));
        }

        return taskObj;
    }

    /// <inheritdoc cref="ICleanArchMediator.PublishAsync{TNotification}(TNotification, CancellationToken)"/>
    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        if (notification is null) throw new ArgumentNullException(nameof(notification));

        var notificationType = typeof(TNotification);
        var handlerServiceType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

        var handlers = _notificationHandlersCache.GetOrAdd(notificationType, _ =>
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(handlerServiceType);
            var resolved = _serviceProvider.GetService(enumerableType) as System.Collections.IEnumerable;

            return resolved?.Cast<object>() ?? Enumerable.Empty<object>();
        });

        var tasks = handlers.Select(h =>
        {
            var handleMethod = h.GetType().GetMethod("HandleAsync");
            if (handleMethod is null)
            {
                throw new InvalidOperationException(DomainMessages.TypeDoesNotImplementHandleAsync.ToFormat(h.GetType().FullName));
            }
            var taskObj = handleMethod.Invoke(h, new object?[] { notification, cancellationToken }) as Task;
            return taskObj ?? Task.CompletedTask;
        });

        await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}