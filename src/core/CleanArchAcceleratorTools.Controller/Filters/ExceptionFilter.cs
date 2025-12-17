using CleanArchAcceleratorTools.Controller.Models;
using CleanArchAcceleratorTools.Domain.Exceptions;
using CleanArchAcceleratorTools.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using CleanArchAcceleratorTools.Domain.Messages;

namespace CleanArchAcceleratorTools.Controller.Filters;

/// <summary>
/// Global exception filter that translates exceptions into API responses and logs details.
/// </summary>
public class ExceptionFilter : IExceptionFilter
{
    private readonly IApplicationLogger _applicationLogger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes the filter with logging and configuration services.
    /// </summary>
    /// <param name="applicationLogger">Application logger used to record errors.</param>
    /// <param name="configuration">Configuration source to control response details.</param>
    public ExceptionFilter(IApplicationLogger applicationLogger, IConfiguration configuration)
    {
        _applicationLogger = applicationLogger;
        _configuration = configuration;
    }

    /// <summary>
    /// Handles an unhandled exception by producing a standardized HTTP response.
    /// </summary>
    /// <param name="context">The exception context containing the error and HTTP pipeline data.</param>
    /// <remarks>
    /// Behavior:
    /// - <see cref="DomainException"/>: 200 OK with validation messages.
    /// - <see cref="UnauthorizedAccessException"/>: 403 Forbid.
    /// - Others: 400 Bad Request with detailed or generic message based on "ApiConfiguration:ShowException".
    /// Also logs the error and marks the exception as handled.
    /// </remarks>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException)
        {
            var domainException = context.Exception as DomainException;

            var notifications = domainException!.ValidationResult?.Errors
                .Select(e => e.ErrorMessage)
                .ToList() ?? new List<string> { domainException.Message };

            context.Result = new OkObjectResult(new ResponseResult
            {
                Success = false,
                Notifications = notifications
            });
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            context.Result = new ForbidResult();
        }
        else
        {
            bool.TryParse(_configuration["ApiConfiguration:ShowException"], out var showException);

            if (showException)
            {
                var exceptionMessage = GetExeptionMessage(context);

                context.Result = new BadRequestObjectResult(new ResponseResult
                {
                    Success = false,
                    Notifications = exceptionMessage.Message.Split(DomainMessages.ExceptionMessageSeparator).Select(m => m.Trim()).ToList()
                });
            }
            else
            {
                var defaultErrorMessage = _configuration["ApiConfiguration:DefaultErrorMessage"] ?? DomainMessages.DefaultUnhandledErrorMessage;
                context.Result = new BadRequestObjectResult(new ResponseResult
                {
                    Success = false,
                    Notifications = new List<string> { defaultErrorMessage }
                });
            }
        }

        LogError(context);
        context.ExceptionHandled = true;
    }

    /// <summary>
    /// Logs the exception message and trace using the application logger.
    /// </summary>
    /// <param name="context">The exception context providing error details.</param>
    private void LogError(ExceptionContext context)
    {
        var exceptionMessage = GetExeptionMessage(context);
        _applicationLogger.LogErrorAsync(exceptionMessage.Message, exceptionMessage.Trace);
    }

    /// <summary>
    /// Builds a combined message and stack trace from the exception and its inner exception (if present).
    /// </summary>
    /// <param name="context">The exception context to extract messages and traces from.</param>
    /// <returns>
    /// A tuple containing:
    /// - <c>Message</c>: combined message (outer and inner, separated by "|||").
    /// - <c>Trace</c>: combined stack trace (outer and inner, separated by "|||").
    /// </returns>
    private (string Message, string Trace) GetExeptionMessage(ExceptionContext context)
    {
        var exceptionMessage = context.Exception.Message;
        var innerExceptionMessage = context.Exception.InnerException?.Message;
        var stackTrace = context.Exception.StackTrace;
        var innerExceptionStackTrace = context.Exception.InnerException?.StackTrace;

        var separator = DomainMessages.ExceptionMessageSeparator;

        var message = string.IsNullOrWhiteSpace(innerExceptionMessage)
                        ? $"{exceptionMessage}"
                        : $"{exceptionMessage} {separator} {innerExceptionMessage}";

        var trace = string.IsNullOrWhiteSpace(innerExceptionMessage)
                        ? $"{stackTrace}"
                        : $"{stackTrace} {separator} {innerExceptionStackTrace}";

        return (message, trace);
    }
}
