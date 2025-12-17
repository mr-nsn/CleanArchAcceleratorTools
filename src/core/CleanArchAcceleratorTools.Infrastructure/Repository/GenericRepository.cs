using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.Interfaces.Repository;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Util;
using CleanArchAcceleratorTools.Infrastructure.Pagination;
using CleanArchAcceleratorTools.Infrastructure.Select;
using CleanArchAcceleratorTools.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Infrastructure.Repository;

/// <summary>
/// Generic EF Core repository implementing <see cref="IGenericRepository{TEntity}"/> for CRUD, querying, projection, pagination, tracking control, and commits.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
/// <remarks>
/// When tracking is disabled, queries use AsNoTracking and commits call <see cref="DetachAll"/>.
/// Dynamic filter/sort validate inputs and may throw; remove operations throw when not found.
/// </remarks>
public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
{
    /// <summary>
    /// EF Core context for data access.
    /// </summary>
    protected DbContext _context;

    /// <summary>
    /// DbContext factory/registrator.
    /// </summary>
    protected IDbContextRegistratorService<DbContext> _registratedFactory;

    /// <summary>
    /// Entity set for queries and persistence.
    /// </summary>
    protected DbSet<TEntity> _dbSet;

    /// <summary>
    /// Indicates if change tracking is enabled.
    /// </summary>
    protected bool _trackingEnabled = true;

    /// <summary>
    /// Initializes the repository.
    /// </summary>
    /// <param name="context">EF Core context.</param>
    /// <param name="registratedFactory">Context registrator/factory.</param>
    public GenericRepository(DbContext context, IDbContextRegistratorService<DbContext> registratedFactory)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
        _registratedFactory = registratedFactory;
    }

    #region Get and Find

    #region async

    /// <inheritdoc/>
    public virtual async Task<PaginationResult<TEntity>> SearchWithPaginationAsync(QueryFilter<TEntity> queryFilter)
    {
        if (queryFilter.DynamicFilter is null) queryFilter.SetDynamicFilter(new DynamicFilter<TEntity>());
        if (queryFilter.DynamicSort is null) queryFilter.SetDynamicSort(new DynamicSort<TEntity>());

        var filteredQuery = _dbSet.AsQueryable();

        if (queryFilter.DynamicFilter is not null)
            filteredQuery = filteredQuery.Where(queryFilter.DynamicFilter.CompileFilter());

        IOrderedQueryable<TEntity> orderedQuery;

        if (queryFilter.DynamicSort is not null)
        {
            var compiledOrders = queryFilter.DynamicSort.CompileSort();

            orderedQuery = filteredQuery.OrderBy(compiledOrders.First().Expression);

            foreach (var order in compiledOrders.Skip(1))
            {
                switch (order.Order)
                {
                    case DynamicSortConstants.SORT_ORDER_ASC:
                        orderedQuery = orderedQuery.ThenBy(order.Expression);
                        break;
                    case DynamicSortConstants.SORT_ORDER_DESC:
                        orderedQuery = orderedQuery.ThenByDescending(order.Expression);
                        break;
                    default:
                        throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
                }
            }
        }
        else
        {
            orderedQuery = filteredQuery.OrderBy(f => f.Id);
        }

        return _trackingEnabled
            ? await orderedQuery
                .DynamicSelect(queryFilter.Fields)
                .GetPagination(queryFilter)
                .ToPaginationResultListAsync()
            : await orderedQuery
                .AsNoTracking()
                .DynamicSelect(queryFilter.Fields)
                .GetPagination(queryFilter)
                .ToPaginationResultListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<ICollection<TEntity>> DynamicSelectAsync(params string[] fields)
    {
        var dynamicFilter = new DynamicFilter<TEntity>();
        var dynamicSort = new DynamicSort<TEntity>();

        var filteredQuery = _dbSet.AsQueryable();

        if (dynamicFilter is not null)
            filteredQuery = filteredQuery.Where(dynamicFilter.CompileFilter());

        IOrderedQueryable<TEntity> orderedQuery;

        if (dynamicSort is not null)
        {
            var compiledOrders = dynamicSort.CompileSort();

            var firstOrder = compiledOrders.First();

            switch (firstOrder.Order)
            {
                case DynamicSortConstants.SORT_ORDER_ASC:
                    orderedQuery = filteredQuery.OrderBy(firstOrder.Expression);
                    break;
                case DynamicSortConstants.SORT_ORDER_DESC:
                    orderedQuery = filteredQuery.OrderByDescending(firstOrder.Expression);
                    break;
                default:
                    throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
            }

            foreach (var order in compiledOrders.Skip(1))
            {
                switch (order.Order)
                {
                    case DynamicSortConstants.SORT_ORDER_ASC:
                        orderedQuery = orderedQuery.ThenBy(order.Expression);
                        break;
                    case DynamicSortConstants.SORT_ORDER_DESC:
                        orderedQuery = orderedQuery.ThenByDescending(order.Expression);
                        break;
                    default:
                        throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
                }
            }
        }
        else
        {
            orderedQuery = filteredQuery.OrderByDescending(f => f.Id);
        }

        return _trackingEnabled
            ? await orderedQuery
                .DynamicSelect(fields)
                .ToListAsync()
            : await orderedQuery
                .AsNoTracking()
                .DynamicSelect(fields)
                .ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<ICollection<TEntity>> DynamicSelectAsync(DynamicFilter<TEntity>? dynamicFilter = null, DynamicSort<TEntity>? dynamicSort = null, params string[] fields)
    {
        if (dynamicFilter is null) dynamicFilter = new DynamicFilter<TEntity>();
        if (dynamicSort is null) dynamicSort = new DynamicSort<TEntity>();

        var filteredQuery = _dbSet.AsQueryable();

        if (dynamicFilter is not null)
            filteredQuery = filteredQuery.Where(dynamicFilter.CompileFilter());

        IOrderedQueryable<TEntity> orderedQuery;

        if (dynamicSort is not null)
        {
            var compiledOrders = dynamicSort.CompileSort();

            var firstOrder = compiledOrders.First();

            switch (firstOrder.Order)
            {
                case DynamicSortConstants.SORT_ORDER_ASC:
                    orderedQuery = filteredQuery.OrderBy(firstOrder.Expression);
                    break;
                case DynamicSortConstants.SORT_ORDER_DESC:
                    orderedQuery = filteredQuery.OrderByDescending(firstOrder.Expression);
                    break;
                default:
                    throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
            }

            foreach (var order in compiledOrders.Skip(1))
            {
                switch (order.Order)
                {
                    case DynamicSortConstants.SORT_ORDER_ASC:
                        orderedQuery = orderedQuery.ThenBy(order.Expression);
                        break;
                    case DynamicSortConstants.SORT_ORDER_DESC:
                        orderedQuery = orderedQuery.ThenByDescending(order.Expression);
                        break;
                    default:
                        throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
                }
            }
        }
        else
        {
            orderedQuery = filteredQuery.OrderByDescending(f => f.Id);
        }

        return _trackingEnabled
            ? await orderedQuery
                .DynamicSelect(fields)
                .ToListAsync()
            : await orderedQuery
                .AsNoTracking()
                .DynamicSelect(fields)
                .ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<ICollection<TEntity>> GetAllAsync()
    {
        return _trackingEnabled
            ? await _dbSet.ToListAsync()
            : await _dbSet.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> GetByIdAsync(long? id)
    {
        return _trackingEnabled
            ? await _dbSet.FirstOrDefaultAsync(x => x.Id == id)
            : await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public virtual async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _trackingEnabled
            ? await _dbSet.Where(predicate).ToListAsync()
            : await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _trackingEnabled
            ? await _dbSet.FirstOrDefaultAsync(predicate)
            : await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    #endregion

    #region sync

    /// <inheritdoc/>
    public PaginationResult<TEntity> SearchWithPagination(QueryFilter<TEntity> queryFilter)
    {
        if (queryFilter.DynamicFilter is null) queryFilter.SetDynamicFilter(new DynamicFilter<TEntity>());
        if (queryFilter.DynamicSort is null) queryFilter.SetDynamicSort(new DynamicSort<TEntity>());

        var filteredQuery = _dbSet.AsQueryable();

        if (queryFilter.DynamicFilter is not null)
            filteredQuery = filteredQuery.Where(queryFilter.DynamicFilter.CompileFilter());

        IOrderedQueryable<TEntity> orderedQuery;

        if (queryFilter.DynamicSort is not null)
        {
            var compiledOrders = queryFilter.DynamicSort.CompileSort();

            orderedQuery = filteredQuery.OrderBy(compiledOrders.First().Expression);

            foreach (var order in compiledOrders.Skip(1))
            {
                switch (order.Order)
                {
                    case DynamicSortConstants.SORT_ORDER_ASC:
                        orderedQuery = orderedQuery.ThenBy(order.Expression);
                        break;
                    case DynamicSortConstants.SORT_ORDER_DESC:
                        orderedQuery = orderedQuery.ThenByDescending(order.Expression);
                        break;
                    default:
                        throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
                }
            }
        }
        else
        {
            orderedQuery = filteredQuery.OrderBy(f => f.Id);
        }

        return _trackingEnabled
            ? orderedQuery
                .DynamicSelect(queryFilter.Fields)
                .GetPagination(queryFilter)
                .ToPaginationResultList()
            : orderedQuery
                .AsNoTracking()
                .DynamicSelect(queryFilter.Fields)
                .GetPagination(queryFilter)
                .ToPaginationResultList();
    }

    /// <inheritdoc/>
    public virtual ICollection<TEntity> DynamicSelect(params string[] fields)
    {
        var dynamicFilter = new DynamicFilter<TEntity>();
        var dynamicSort = new DynamicSort<TEntity>();

        var filteredQuery = _dbSet.AsQueryable();

        if (dynamicFilter is not null)
            filteredQuery = filteredQuery.Where(dynamicFilter.CompileFilter());

        IOrderedQueryable<TEntity> orderedQuery;

        if (dynamicSort is not null)
        {
            var compiledOrders = dynamicSort.CompileSort();

            var firstOrder = compiledOrders.First();

            switch (firstOrder.Order)
            {
                case DynamicSortConstants.SORT_ORDER_ASC:
                    orderedQuery = filteredQuery.OrderBy(firstOrder.Expression);
                    break;
                case DynamicSortConstants.SORT_ORDER_DESC:
                    orderedQuery = filteredQuery.OrderByDescending(firstOrder.Expression);
                    break;
                default:
                    throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
            }

            foreach (var order in compiledOrders.Skip(1))
            {
                switch (order.Order)
                {
                    case DynamicSortConstants.SORT_ORDER_ASC:
                        orderedQuery = orderedQuery.ThenBy(order.Expression);
                        break;
                    case DynamicSortConstants.SORT_ORDER_DESC:
                        orderedQuery = orderedQuery.ThenByDescending(order.Expression);
                        break;
                    default:
                        throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
                }
            }
        }
        else
        {
            orderedQuery = filteredQuery.OrderByDescending(f => f.Id);
        }

        return _trackingEnabled
            ? orderedQuery
                .DynamicSelect(fields)
                .ToList()
            : orderedQuery
                .AsNoTracking()
                .DynamicSelect(fields)
                .ToList();
    }

    /// <inheritdoc/>
    public virtual ICollection<TEntity> DynamicSelect(DynamicFilter<TEntity>? dynamicFilter = null, DynamicSort<TEntity>? dynamicSort = null, params string[] fields)
    {
        if (dynamicFilter is null) dynamicFilter = new DynamicFilter<TEntity>();
        if (dynamicSort is null) dynamicSort = new DynamicSort<TEntity>();

        var filteredQuery = _dbSet.AsQueryable();

        if (dynamicFilter is not null && dynamicFilter.ValidationResult.IsValid)
            filteredQuery = filteredQuery.Where(dynamicFilter.CompileFilter());

        IOrderedQueryable<TEntity> orderedQuery;

        if (dynamicSort is not null)
        {
            var compiledOrders = dynamicSort.CompileSort();

            var firstOrder = compiledOrders.First();

            switch (firstOrder.Order)
            {
                case DynamicSortConstants.SORT_ORDER_ASC:
                    orderedQuery = filteredQuery.OrderBy(firstOrder.Expression);
                    break;
                case DynamicSortConstants.SORT_ORDER_DESC:
                    orderedQuery = filteredQuery.OrderByDescending(firstOrder.Expression);
                    break;
                default:
                    throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
            }

            foreach (var order in compiledOrders.Skip(1))
            {
                switch (order.Order)
                {
                    case DynamicSortConstants.SORT_ORDER_ASC:
                        orderedQuery = orderedQuery.ThenBy(order.Expression);
                        break;
                    case DynamicSortConstants.SORT_ORDER_DESC:
                        orderedQuery = orderedQuery.ThenByDescending(order.Expression);
                        break;
                    default:
                        throw new InvalidOperationException(DomainMessages.InvalidOrderDirectionInDynamicSort);
                }
            }
        }
        else
        {
            orderedQuery = filteredQuery.OrderByDescending(f => f.Id);
        }

        return _trackingEnabled
            ? orderedQuery
                .DynamicSelect(fields)
                .ToList()
            : orderedQuery
                .AsNoTracking()
                .DynamicSelect(fields)
                .ToList();
    }

    /// <inheritdoc/>
    public virtual ICollection<TEntity> GetAll()
    {
        return _trackingEnabled
            ? _dbSet.ToList()
            : _dbSet.AsNoTracking().ToList();
    }

    /// <inheritdoc/>
    public virtual TEntity? GetById(long? id)
    {
        return _trackingEnabled
            ? _dbSet.FirstOrDefault(x => x.Id == id)
            : _dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
    }

    /// <inheritdoc/>
    public virtual ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
    {
        return _trackingEnabled
            ? _dbSet.Where(predicate).ToList()
            : _dbSet.AsNoTracking().Where(predicate).ToList();
    }

    /// <inheritdoc/>
    public virtual TEntity? FindFirst(Expression<Func<TEntity, bool>> predicate)
    {
        return _trackingEnabled
            ? _dbSet.FirstOrDefault(predicate)
            : _dbSet.AsNoTracking().FirstOrDefault(predicate);
    }

    #endregion

    #endregion

    #region Add

    #region async

    /// <inheritdoc/>
    public virtual async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> AddAndCommitAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await CommitAsync();
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task AddRangeAsync(ICollection<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    /// <inheritdoc/>
    public virtual async Task<ICollection<TEntity>> AddRangeAndCommitAsync(ICollection<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await CommitAsync();
        return entities;
    }

    #endregion

    #region sync

    /// <inheritdoc/>
    public virtual void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    /// <inheritdoc/>
    public virtual TEntity AddAndCommit(TEntity entity)
    {
        _dbSet.Add(entity);
        Commit();
        return entity;
    }

    /// <inheritdoc/>
    public virtual void AddRange(ICollection<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    /// <inheritdoc/>
    public virtual ICollection<TEntity> AddRangeAndCommit(ICollection<TEntity> entities)
    {
        _dbSet.AddRange(entities);
        Commit();
        return entities;
    }

    #endregion

    #endregion

    #region Update

    #region async

    /// <inheritdoc/>
    public virtual async Task UpdateAsync(TEntity entity)
    {
        await Task.Run(() =>
        {
            _dbSet.Update(entity);
        });
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity> UpdateAndCommitAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await CommitAsync();
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task UpdateRangeAsync(ICollection<TEntity> entities)
    {
        await Task.Run(() =>
        {
            _dbSet.UpdateRange(entities);
        });
    }

    /// <inheritdoc/>
    public virtual async Task<ICollection<TEntity>> UpdateRangeAndCommitAsync(ICollection<TEntity> entities)
    {
        await Task.Run(() =>
        {
            _dbSet.UpdateRange(entities);
        });

        await CommitAsync();
        return entities;
    }

    #endregion

    #region sync

    /// <inheritdoc/>
    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    /// <inheritdoc/>
    public virtual TEntity UpdateAndCommit(TEntity entity)
    {
        _dbSet.Update(entity);
        Commit();
        return entity;
    }

    /// <inheritdoc/>
    public virtual void UpdateRange(ICollection<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    /// <inheritdoc/>
    public virtual ICollection<TEntity> UpdateRangeAndCommit(ICollection<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
        Commit();
        return entities;
    }

    #endregion

    #endregion

    #region Remove

    #region async

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(long? id)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException(DomainMessages.EntityWithIdNotFound.ToFormat(id));
        _dbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAndCommitAsync(long? id)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException(DomainMessages.EntityWithIdNotFound.ToFormat(id));
        _dbSet.Remove(entity);
        await CommitAsync();
    }

    /// <inheritdoc/>
    public virtual async Task RemoveRangeAsync(params long?[] ids)
    {
        var entities = await FindAllAsync(x => ids != null && ids.Contains(x.Id)) ?? throw new KeyNotFoundException(DomainMessages.SomeEntitiesWereNotFoundIds.ToFormat(string.Join(", ", ids ?? Array.Empty<long?>())));
        _dbSet.RemoveRange(entities);
    }

    /// <inheritdoc/>
    public virtual async Task RemoveRangeAndCommitAsync(params long?[] ids)
    {
        var entities = await FindAllAsync(x => ids != null && ids.Contains(x.Id)) ?? throw new KeyNotFoundException(DomainMessages.SomeEntitiesWereNotFoundIds.ToFormat(string.Join(", ", ids ?? Array.Empty<long?>())));
        _dbSet.RemoveRange(entities);
        await CommitAsync();
    }

    #endregion

    #region sync

    /// <inheritdoc/>
    public virtual void Remove(long? id)
    {
        var entity = GetById(id) ?? throw new KeyNotFoundException(DomainMessages.EntityWithIdNotFound.ToFormat(id));
        _dbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual void RemoveAndCommit(long? id)
    {
        var entity = GetById(id) ?? throw new KeyNotFoundException(DomainMessages.EntityWithIdNotFound.ToFormat(id));
        _dbSet.Remove(entity);
        Commit();
    }

    /// <inheritdoc/>
    public virtual void RemoveRange(params long?[] ids)
    {
        var entities = FindAll(x => ids != null && ids.Contains(x.Id)) ?? throw new KeyNotFoundException(DomainMessages.SomeEntitiesWereNotFoundIds.ToFormat(string.Join(", ", ids ?? Array.Empty<long?>())));
        _dbSet.RemoveRange(entities);
    }

    /// <inheritdoc/>
    public virtual void RemoveRangeAndCommit(params long?[] ids)
    {
        var entities = FindAll(x => ids != null && ids.Contains(x.Id)) ?? throw new KeyNotFoundException(DomainMessages.SomeEntitiesWereNotFoundIds.ToFormat(string.Join(", ", ids ?? Array.Empty<long?>())));
        _dbSet.RemoveRange(entities);
        Commit();
    }

    #endregion

    #endregion

    #region Any

    #region async

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    #endregion

    #region sync

    /// <inheritdoc/>
    public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Any(predicate);
    }

    #endregion

    #endregion

    #region Count

    #region async

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }

    #endregion

    #region sync

    /// <inheritdoc/>
    public virtual int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Count(predicate);
    }

    #endregion

    #endregion

    #region Detach

    /// <inheritdoc/>
    public virtual void Detach(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }

    /// <inheritdoc/>
    public virtual void DetachAll()
    {
        var entityEntries = _context.ChangeTracker.Entries().ToList();

        foreach (EntityEntry entityEntry in entityEntries)
        {
            entityEntry.State = EntityState.Detached;
        }
    }

    #endregion

    #region Change Tracking

    /// <inheritdoc/>
    public virtual void DisableChangeTracker()
    {
        _context.ChangeTracker.AutoDetectChangesEnabled = false;
        _trackingEnabled = false;
    }

    /// <inheritdoc/>
    public virtual void EnableChangeTracker()
    {
        _context.ChangeTracker.AutoDetectChangesEnabled = true;
        _trackingEnabled = true;
    }

    #endregion

    #region Commit

    #region async

    /// <inheritdoc/>
    public virtual async Task<int> CommitAsync()
    {
        var changesCounter = await _context.SaveChangesAsync();
        if (!_trackingEnabled) DetachAll();
        return changesCounter;
    }

    #endregion

    #region sync

    /// <inheritdoc/>
    public virtual int Commit()
    {
        var changesCounter = _context.SaveChanges();
        if (!_trackingEnabled) DetachAll();
        return changesCounter;
    }

    #endregion

    #endregion

    #region Dispose

    /// <inheritdoc/>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion
}