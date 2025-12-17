using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Models.Enums;
using CleanArchAcceleratorTools.Domain.Util;
using System;
using System.Collections.Concurrent;

namespace CleanArchAcceleratorTools.Infrastructure.ParallelProcessing;

/// <summary>
/// Configurable engine to execute repeatable tasks in parallel batches with logging and exception handling.
/// </summary>
internal class ParallelTasksProcessing
{
    /// <summary>
    /// Total number of records to process.
    /// </summary>
    public int TotalRegisters { get; private set; }

    /// <summary>
    /// Number of records per batch.
    /// </summary>
    public int BatchSize { get; private set; }

    /// <summary>
    /// Maximum concurrent threads.
    /// </summary>
    public int MaxDegreeOfParallelism { get; private set; }

    /// <summary>
    /// Maximum processes per thread.
    /// </summary>
    public int MaxDegreeOfProcessesPerThread { get; private set; }

    /// <summary>
    /// Total number of required batches.
    /// </summary>
    public int TotalBatches { get; private set; }

    /// <summary>
    /// Mapping of start index to end index for batch ranges.
    /// </summary>
    public Dictionary<int, int> StartsAndEnds { get; private set; }

    private IApplicationLogger? _logger;
    private bool _logEnabled = false;

    private List<Type> _ignoredExceptions = new List<Type>();
    private List<Exception> _throwedExceptions = new List<Exception>();
    private ParallelExceptionBehavior _exceptionBehavior = ParallelExceptionBehavior.IgnoreAllExeptions;
    private bool _showExceptions = false;

    /// <summary>
    /// Initializes with parallel processing parameters.
    /// </summary>
    /// <param name="parallelParams">Parameters controlling parallel execution.</param>
    /// <exception cref="ArgumentException">Thrown if any parameter is less than 1.</exception>
    public ParallelTasksProcessing(ParallelParams parallelParams)
    {
        if (parallelParams.TotalRegisters < 1 || parallelParams.BatchSize < 1 || parallelParams.MaximumDegreeOfParalelism < 1 || parallelParams.MaxDegreeOfProcessesPerThread < 1)
            throw new ArgumentException(DomainMessages.ParallelParametersMustBeGreaterThanZero.ToFormat(
                nameof(parallelParams.BatchSize),
                nameof(parallelParams.MaxDegreeOfProcessesPerThread),
                nameof(parallelParams.MaximumDegreeOfParalelism),
                nameof(parallelParams.TotalRegisters)));

        TotalRegisters = parallelParams.TotalRegisters;
        BatchSize = parallelParams.BatchSize;
        MaxDegreeOfParallelism = parallelParams.MaximumDegreeOfParalelism;
        MaxDegreeOfProcessesPerThread = parallelParams.MaxDegreeOfProcessesPerThread;
        TotalBatches = CalculateTotalBatches();
        StartsAndEnds = CalculateStartsAndEnds();
    }

    /// <summary>
    /// Calculates total batches.
    /// </summary>
    private int CalculateTotalBatches()
    {
        return (int)Math.Ceiling((decimal)TotalRegisters / (decimal)BatchSize);
    }

    /// <summary>
    /// Calculates start and end indices for each batch range.
    /// </summary>
    private Dictionary<int, int> CalculateStartsAndEnds()
    {
        var starts = new Dictionary<int, int>();

        for (int start = 0; start < TotalBatches; start += MaxDegreeOfProcessesPerThread)
            starts.Add(start, 0);

        foreach (var currentStart in starts)
        {
            var nextStart = starts.Keys
                .OfType<int?>()
                .Where(x => x > currentStart.Key)
                .OrderBy(x => x)
                .FirstOrDefault();

            var end = nextStart.HasValue ? nextStart.Value : TotalBatches;
            starts[currentStart.Key] = end;
        }

        return starts;
    }

    /// <summary>
    /// Enables logging with the provided logger.
    /// </summary>
    /// <param name="logger">Logger for events and errors.</param>
    public void EnableLog(IApplicationLogger logger)
    {
        _logger = logger;
        _logEnabled = true;
    }

    /// <summary>
    /// Adds exception types to ignore during execution.
    /// </summary>
    /// <param name="exceptions">Exception types to ignore.</param>
    public void EnableIgnoredExceptions(ICollection<Type> exceptions)
    {
        _ignoredExceptions.AddRange(exceptions.ToList());
    }

    /// <summary>
    /// Sets exception handling behavior.
    /// </summary>
    /// <param name="behavior">How exceptions are treated.</param>
    public void SetExceptionBehavior(ParallelExceptionBehavior behavior)
    {
        _exceptionBehavior = behavior;
    }

    /// <summary>
    /// Enables exception display in logs.
    /// </summary>
    public void EnableShowExceptions()
    {
        _showExceptions = true;
    }

    /// <summary>
    /// Returns exceptions thrown during execution.
    /// </summary>
    /// <returns>Collected exceptions.</returns>
    public ICollection<Exception> GetExceptions()
    {
        return _throwedExceptions;
    }

    /// <summary>
    /// Executes a repeatable task over batch ranges in parallel and aggregates results.
    /// </summary>
    /// <typeparam name="T">Result type produced by the task.</typeparam>
    /// <param name="repeatableTask">Function receiving a 1-based batch index and returning items for that batch.</param>
    /// <returns>A task with the concatenated results from all batches.</returns>
    public async Task<ICollection<T>> DoParallelAsync<T>(Func<int, Task<ICollection<T>>> repeatableTask)
    {
        var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = MaxDegreeOfParallelism };
        var returns = new ConcurrentBag<T>();

        await WriteTraceLog(DomainMessages.TraceInitializingParallelism, DomainMessages.TraceParallelismDetails.ToFormat(MaxDegreeOfParallelism, MaxDegreeOfProcessesPerThread));
        await WriteTraceLog(DomainMessages.TraceTotalRecords.ToFormat(TotalRegisters), string.Empty);
        await WriteTraceLog(DomainMessages.TraceBatchSize.ToFormat(BatchSize), string.Empty);
        await WriteTraceLog(DomainMessages.TraceTotalRequiredIterations.ToFormat(TotalBatches), string.Empty);

        try
        {
            await Parallel.ForEachAsync(StartsAndEnds, parallelOptions, async (startAndEnd, _) =>
            {
                var exceptions = new ConcurrentQueue<Exception>();

                try
                {
                    await WriteTraceLog(DomainMessages.TraceProcessingIterationRange, DomainMessages.TraceProcessingIterationRangeDetails.ToFormat(startAndEnd.Key + 1, startAndEnd.Value, TotalBatches));

                    var tasks = RepeatTask(repeatableTask, startAndEnd.Key, startAndEnd.Value).ToList();

                    await foreach (var registers in TaskUtils.WhenEach(tasks.ToArray()))
                    {
                        await WriteTraceLog(DomainMessages.TraceQueryFinished, DomainMessages.TraceIterationLabel.ToFormat(0)); // Todo: Replace 0 with the actual iteration number
                        registers.ToList().ForEach(r => returns.Add(r));
                    }
                }
                catch (Exception e)
                {
                    _throwedExceptions.Add(e);

                    if (_exceptionBehavior == ParallelExceptionBehavior.StopOnFirstException)
                        throw;

                    exceptions.Enqueue(e);
                    await WriteErrorLog(e);
                }

                if (!exceptions.IsEmpty)
                    throw new AggregateException(exceptions);
            });
        }
        catch (AggregateException ae)
        {
            if (_exceptionBehavior == ParallelExceptionBehavior.IgnoreAllExeptions)
                return returns.ToList();

            var notIgnoredExceptions = new List<Exception>();

            foreach (var ex in ae.Flatten().InnerExceptions)
            {
                if (!_ignoredExceptions.Any(x => x == ex.GetType()))
                    notIgnoredExceptions.Add(ex);
            }

            if (notIgnoredExceptions.Count > 0)
                throw new AggregateException(notIgnoredExceptions);
        }

        return returns.ToList();
    }

    /// <summary>
    /// Creates tasks for the specified 0-based batch range.
    /// </summary>
    private IEnumerable<Task<ICollection<T>>> RepeatTask<T>(Func<int, Task<ICollection<T>>> repeatableTask, int start, int end)
    {
        for (var i = start; i < end; i++)
            yield return repeatableTask(i + 1);
    }

    /// <summary>
    /// Logs an error with exception details.
    /// </summary>
    private async Task WriteErrorLog(Exception ex)
    {
        if (!_logEnabled || _logger is null)
            return;

        var details = GetExceptionDetails(ex).Split(new[] { DomainMessages.ExceptionMessageSeparator }, StringSplitOptions.None);
        await _logger.LogErrorAsync(details[0], details.Length > 1 ? details[1] : string.Empty);
    }

    /// <summary>
    /// Logs an informational trace message.
    /// </summary>
    private async Task WriteTraceLog(string message, string details)
    {
        if (!_logEnabled || _logger is null)
            return;

        await _logger.LogTraceAsync(message, details);
    }

    /// <summary>
    /// Get the exception message and stack trace, including inner exceptions if present.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns>The exception message and stack trace.</returns>
    private string GetExceptionDetails(Exception ex)
    {
        var message = ex.Message;
        var innerExceptionMessage = ex.InnerException?.Message;
        var stackTrace = ex.StackTrace;
        var innerExceptionStackTrace = ex.InnerException?.StackTrace;

        var fullMessage = string.IsNullOrWhiteSpace(innerExceptionMessage)
                ? $"{message}"
                : $"{message} / {innerExceptionMessage}";

        var trace = string.IsNullOrWhiteSpace(innerExceptionMessage)
                    ? $"{stackTrace}"
                    : $"{stackTrace} / {innerExceptionStackTrace}";

        return $"{fullMessage} {DomainMessages.ExceptionMessageSeparator} {trace}";
    }
}