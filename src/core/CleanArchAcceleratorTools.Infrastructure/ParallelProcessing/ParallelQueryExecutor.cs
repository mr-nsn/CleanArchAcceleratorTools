using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Models;
using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Infrastructure.ParallelProcessing;

/// <summary>
/// Utilities to run an <see cref="IQueryable{T}"/> in parallel batches with configurable parallelism and optional logging.
/// </summary>
/// <remarks>
/// - Partitions by <see cref="ParallelParams.BatchSize"/> and executes pages in parallel respecting <see cref="ParallelParams.MaximumDegreeOfParalelism"/> and <see cref="ParallelParams.MaxDegreeOfProcessesPerThread"/>.
/// - If <paramref name="parallelParams"/> is null, defaults are inferred (count, batch size 1000, CPU-bound parallelism).
/// - For deterministic results, ensure a stable ordering before execution.
/// </remarks>
public static class ParallelQueryExecutor
{
    /// <summary>
    /// Runs the query in parallel batches using the provided <see cref="ParallelParams"/>; optionally logs progress.
    /// </summary>
    /// <typeparam name="T">Element type produced by the query. Must be a class.</typeparam>
    /// <param name="query">Expression that yields the <see cref="IQueryable{T}"/> to process (compiled per page).</param>
    /// <param name="parallelParams">
    /// Optional parallel settings. When null, defaults are used:
    /// - <see cref="ParallelParams.TotalRegisters"/>: computed via <c>query.Compile()().Count()</c>
    /// - <see cref="ParallelParams.BatchSize"/>: 1000
    /// - <see cref="ParallelParams.MaximumDegreeOfParalelism"/>: <see cref="Environment.ProcessorCount"/>
    /// - <see cref="ParallelParams.MaxDegreeOfProcessesPerThread"/>: 1
    /// </param>
    /// <param name="logger">Optional <see cref="IApplicationLogger"/> for info/warn/error/debug/trace.</param>
    /// <returns>A task producing all concatenated items from processed batches.</returns>
    /// <remarks>The query is evaluated per page via Skip/Take.</remarks>
    public static async Task<ICollection<T>> DoItParallelAsync<T>(Expression<Func<IQueryable<T>>> query, ParallelParams? parallelParams = null, IApplicationLogger? logger = null) where T : class
    {
        if (parallelParams is null)
        {
            parallelParams = new ParallelParams
            {
                TotalRegisters = query.Compile()().Count(),
                BatchSize = 1000,
                MaximumDegreeOfParalelism = Environment.ProcessorCount,
                MaxDegreeOfProcessesPerThread = 1
            };
        }

        var parallelTaskProcessing = new ParallelTasksProcessing(parallelParams);
        if (logger is not null) parallelTaskProcessing.EnableLog(logger);
        return await parallelTaskProcessing.DoParallelAsync((int page) => ExecuteSearchAsync(query, page, parallelParams));
    }

    /// <summary>
    /// Executes a single page using the configured batch size.
    /// </summary>
    /// <typeparam name="T">Element type of the query.</typeparam>
    /// <param name="query">Expression returning the <see cref="IQueryable{T}"/>.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="parallelParams">Parameters providing the batch size.</param>
    /// <returns>A task with the page items.</returns>
    private static async Task<ICollection<T>> ExecuteSearchAsync<T>(Expression<Func<IQueryable<T>>> query, int page, ParallelParams parallelParams)
    {
        var pageSize = parallelParams.BatchSize;
        var skip = (page - 1) * parallelParams.BatchSize;

        var resultado = query.Compile()()
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return await Task.FromResult(resultado ?? Enumerable.Empty<T>().ToList());
    }
}
