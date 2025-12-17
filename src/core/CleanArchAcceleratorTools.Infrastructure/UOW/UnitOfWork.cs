using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Infrastructure.UOW;

/// <summary>
/// Lightweight <see cref="IUnitOfWork"/> coordinating multiple <see cref="DbContext"/> instances.
/// </summary>
/// <remarks>
/// Registers contexts by key, supports global/per-key commits, provides transaction management via the primary context,
/// and uses thread-safe registration with <see cref="ConcurrentDictionary{TKey, TValue}"/>.
/// </remarks>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Primary context for transactions and global commits.
    /// </summary>
    private readonly DbContext _context;

    /// <summary>
    /// Thread-safe registry of contexts grouped by key.
    /// </summary>
    private readonly ConcurrentDictionary<string, List<DbContext>> _registeredContexts;

    /// <summary>
    /// Synchronization lock for list updates.
    /// </summary>
    private readonly object _contextLock = new object();

    /// <summary>
    /// Initializes the unit of work with a primary <see cref="DbContext"/>.
    /// </summary>
    /// <param name="context">Primary EF Core context.</param>
    public UnitOfWork(DbContext context)
    {
        _context = context;
        _registeredContexts = new ConcurrentDictionary<string, List<DbContext>>();
    }

    /// <inheritdoc/>
    public bool AddContextAsync(string key, DbContext context)
    {
        lock (_contextLock)
        {
            _registeredContexts.AddOrUpdate(key, new List<DbContext>() { context }, (k, v) => GetUpdatedContextList(v ?? new List<DbContext>(), context));
            return true;
        }
    }

    /// <inheritdoc/>
    public List<DbContext> GetAllDbContext(string key)
    {
        if (!_registeredContexts.ContainsKey(key) || _registeredContexts[key] is null)
            return new List<DbContext>();

        var contexts = _registeredContexts[key];

        return contexts.ToList();
    }

    /// <inheritdoc/>
    public DbContext? GetFirstDbContext(string key)
    {
        if (!_registeredContexts.ContainsKey(key) || _registeredContexts[key] is null)
            return null;

        var contexts = _registeredContexts[key];

        return contexts.FirstOrDefault();
    }

    /// <inheritdoc/>
    public async Task DiscartAllDbContextAsync()
    {
        if (_registeredContexts.Count == 0) return;

        foreach (var registeredContext in _registeredContexts)
        {
            var contexts = registeredContext.Value;
            if (contexts is null || contexts.Count == 0) return;

            foreach (var context in contexts)
            {
                try
                {
                    await context.DisposeAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        _registeredContexts.Clear();
    }

    /// <inheritdoc/>
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc/>
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<int> CommitAsync(string key)
    {
        if (!_registeredContexts.ContainsKey(key) || _registeredContexts[key] is null)
            throw new KeyNotFoundException(DomainMessages.NoRegisteredContextFoundForKey.ToFormat(key));

        var workingContexts = _registeredContexts[key];

        if (workingContexts == null || workingContexts.Count == 0)
            throw new InvalidOperationException(DomainMessages.NoWorkingContexts);

        var changes = 0;

        try
        {
            foreach (var context in workingContexts)
                changes += await context.SaveChangesAsync();

            return changes;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Returns a new list including existing contexts plus the provided one.
    /// </summary>
    /// <param name="existingValue">Existing registered contexts.</param>
    /// <param name="dataContext">Context to append.</param>
    /// <returns>New list containing all contexts.</returns>
    private List<DbContext> GetUpdatedContextList(List<DbContext> existingValue, DbContext dataContext)
    {
        return new List<DbContext>(existingValue.Concat(new List<DbContext> { dataContext }).ToList());
    }
}