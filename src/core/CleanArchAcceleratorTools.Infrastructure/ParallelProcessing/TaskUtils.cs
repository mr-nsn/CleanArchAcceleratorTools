namespace CleanArchAcceleratorTools.Infrastructure.ParallelProcessing;

/// <summary>
/// Utilities for processing collections of async tasks, yielding results as they finish.
/// </summary>
internal static class TaskUtils
{
    /// <summary>
    /// Yields each task's result as soon as it completes (original order not preserved).
    /// </summary>
    /// <typeparam name="TResult">Result type produced by each task.</typeparam>
    /// <param name="tasks">Tasks to observe.</param>
    /// <returns>An <see cref="IAsyncEnumerable{TResult}"/> yielding results as tasks complete.</returns>
    public static async IAsyncEnumerable<TResult> WhenEach<TResult>(Task<TResult>[] tasks)
    {
        foreach (var bucket in Interleaved(tasks))
        {
            var t = await bucket;
            yield return await t;
        }
    }

    /// <summary>
    /// Produces tasks that complete in the order the input tasks finish.
    /// </summary>
    /// <typeparam name="T">Result type produced by each task.</typeparam>
    /// <param name="tasks">Tasks to observe.</param>
    /// <returns>An array of tasks completing as input tasks finish.</returns>
    private static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
    {
        var inputTasks = tasks.ToList();

        var buckets = new TaskCompletionSource<Task<T>>[inputTasks.Count];
        var results = new Task<Task<T>>[buckets.Length];
        for (int i = 0; i < buckets.Length; i++)
        {
            buckets[i] = new TaskCompletionSource<Task<T>>();
            results[i] = buckets[i].Task;
        }

        int nextTaskIndex = -1;
        Action<Task<T>> continuation = completed =>
        {
            var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
            bucket.TrySetResult(completed);
        };

        foreach (var inputTask in inputTasks)
            inputTask.ContinueWith(continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

        return results;
    }
}
