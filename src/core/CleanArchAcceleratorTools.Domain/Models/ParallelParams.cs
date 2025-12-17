namespace CleanArchAcceleratorTools.Domain.Models;

/// <summary>
/// Lightweight configuration for parallel processing: total items, batch size, max parallelism, and per-thread limits.
/// </summary>
/// <remarks>
/// No runtime validation is enforced. Recommended:
/// - Values >= 1.
/// - <see cref="BatchSize"/> <= <see cref="TotalRegisters"/>.
/// - <see cref="MaxDegreeOfProcessesPerThread"/> aligns with or divides <see cref="BatchSize"/>.
/// </remarks>
public class ParallelParams
{
    /// <summary>
    /// Total number of records/items to process.
    /// </summary>
    public int TotalRegisters { get; set; }

    /// <summary>
    /// Number of items per batch when partitioning the workload.
    /// </summary>
    public int BatchSize { get; set; }

    /// <summary>
    /// Maximum number of concurrent threads/tasks.
    /// </summary>
    public int MaximumDegreeOfParalelism { get; set; }

    /// <summary>
    /// Maximum items processed per thread/task within a batch.
    /// </summary>
    public int MaxDegreeOfProcessesPerThread { get; set; }
}
