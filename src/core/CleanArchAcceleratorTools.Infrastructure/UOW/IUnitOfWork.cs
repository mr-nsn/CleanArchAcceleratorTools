using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchAcceleratorTools.Infrastructure.UOW;

/// <summary>
/// Minimal unit of work contract coordinating context registration, transactions, and commits across <see cref="DbContext"/> instances.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Registers a <see cref="DbContext"/> under a logical key for reuse.
    /// </summary>
    /// <param name="key">Logical key (e.g., tenant or bounded context).</param>
    /// <param name="context">Context instance to register.</param>
    /// <returns><c>true</c> if registered; otherwise, <c>false</c>.</returns>
    bool AddContextAsync(string key, DbContext context);

    /// <summary>
    /// Starts a transaction across registered contexts (provider-dependent).
    /// </summary>
    /// <returns>An <see cref="IDbContextTransaction"/>.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Saves changes for all registered contexts.
    /// </summary>
    /// <returns>Total written entries.</returns>
    Task<int> CommitAsync();

    /// <summary>
    /// Saves changes for contexts registered under the given key.
    /// </summary>
    /// <param name="key">Logical key to commit.</param>
    /// <returns>Total written entries for the key.</returns>
    Task<int> CommitAsync(string key);

    /// <summary>
    /// Discards and detaches all registered contexts.
    /// </summary>
    /// <returns>A task representing the operation.</returns>
    Task DiscartAllDbContextAsync();

    /// <summary>
    /// Retrieves all contexts registered under the key.
    /// </summary>
    /// <param name="key">Logical key used during registration.</param>
    /// <returns>List of matching contexts.</returns>
    List<DbContext> GetAllDbContext(string key);

    /// <summary>
    /// Retrieves the first context registered under the key.
    /// </summary>
    /// <param name="key">Logical key used during registration.</param>
    /// <returns>The first matching context or <c>null</c>.</returns>
    DbContext? GetFirstDbContext(string key);
}
