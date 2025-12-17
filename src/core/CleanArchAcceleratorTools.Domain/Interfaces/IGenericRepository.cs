using CleanArchAcceleratorTools.Domain.Models;
using System.Linq.Expressions;

namespace CleanArchAcceleratorTools.Domain.Interfaces.Repository;

/// <summary>
/// Generic repository for CRUD, queries, pagination, projection, change tracking, and commits.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
public interface IGenericRepository<TEntity> : IDisposable where TEntity : Entity
{
    #region Get and Find

    #region Async

    /// <summary>
    /// Executes a paginated query using the provided filter (fields, sort, and predicates).
    /// </summary>
    /// <param name="queryFilter">Pagination, field projection, filter, and sort options.</param>
    /// <returns>A task returning <see cref="PaginationResult{TEntity}"/> with items and metadata.</returns>
    Task<PaginationResult<TEntity>> SearchWithPaginationAsync(QueryFilter<TEntity> queryFilter);

    /// <summary>
    /// Projects entities to the specified fields with optional dynamic filter and sort.
    /// </summary>
    /// <param name="fields">Field names to include in the result.</param>
    /// <returns>A task returning the projected entities.</returns>
    Task<ICollection<TEntity>> DynamicSelectAsync(params string[] fields);

    /// <summary>
    /// Projects entities to the specified fields with optional dynamic filter and sort.
    /// </summary>
    /// <param name="dynamicFilter">Optional dynamic filter.</param>
    /// <param name="dynamicSort">Optional dynamic sort.</param>
    /// <param name="fields">Field names to include in the result.</param>
    /// <returns>A task returning the projected entities.</returns>
    Task<ICollection<TEntity>> DynamicSelectAsync(DynamicFilter<TEntity>? dynamicFilter = null, DynamicSort<TEntity>? dynamicSort = null, params string[] fields);

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A task returning all entities.</returns>
    Task<ICollection<TEntity>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity by id.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <returns>A task returning the entity or <c>null</c>.</returns>
    Task<TEntity?> GetByIdAsync(long? id);

    /// <summary>
    /// Finds all entities matching a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns>A task returning matching entities.</returns>
    Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Finds the first entity matching a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns>A task returning the entity or <c>null</c>.</returns>
    Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #region Sync

    /// <summary>
    /// Executes a paginated query using the provided filter (fields, sort, and predicates).
    /// </summary>
    /// <param name="queryFilter">Pagination, field projection, filter, and sort options.</param>
    /// <returns><see cref="PaginationResult{TEntity}"/> with items and metadata.</returns>
    PaginationResult<TEntity> SearchWithPagination(QueryFilter<TEntity> queryFilter);

    /// <summary>
    /// Projects entities to the specified fields with optional dynamic filter and sort.
    /// </summary>
    /// <param name="fields">Field names to include in the result.</param>
    /// <returns>Projected entities.</returns>
    ICollection<TEntity> DynamicSelect(params string[] fields);

    /// <summary>
    /// Projects entities to the specified fields with optional dynamic filter and sort.
    /// </summary>
    /// <param name="dynamicFilter">Optional dynamic filter.</param>
    /// <param name="dynamicSort">Optional dynamic sort.</param>
    /// <param name="fields">Field names to include in the result.</param>
    /// <returns>Projected entities.</returns>
    ICollection<TEntity> DynamicSelect(DynamicFilter<TEntity>? dynamicFilter = null, DynamicSort<TEntity>? dynamicSort = null, params string[] fields);    

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>All entities.</returns>
    ICollection<TEntity> GetAll();

    /// <summary>
    /// Retrieves an entity by id.
    /// </summary>
    /// <param name="id">Entity id.</param>
    /// <returns>The entity or <c>null</c>.</returns>
    TEntity? GetById(long? id);

    /// <summary>
    /// Finds all entities matching a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns>Matching entities.</returns>
    ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// Finds the first entity matching a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns>The entity or <c>null</c>.</returns>
    TEntity? FindFirst(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #endregion

    #region Add

    #region Async

    /// <summary>
    /// Adds an entity.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Adds an entity and commits.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <returns>A task returning the added entity.</returns>
    Task<TEntity> AddAndCommitAsync(TEntity entity);

    /// <summary>
    /// Adds entities.
    /// </summary>
    /// <param name="entities">Entities to add.</param>
    Task AddRangeAsync(ICollection<TEntity> entities);

    /// <summary>
    /// Adds entities and commits.
    /// </summary>
    /// <param name="entities">Entities to add.</param>
    /// <returns>A task returning the added entities.</returns>
    Task<ICollection<TEntity>> AddRangeAndCommitAsync(ICollection<TEntity> entities);

    #endregion

    #region Sync

    /// <summary>
    /// Adds an entity.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    void Add(TEntity entity);

    /// <summary>
    /// Adds an entity and commits.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <returns>The added entity.</returns>
    TEntity AddAndCommit(TEntity entity);

    /// <summary>
    /// Adds entities.
    /// </summary>
    /// <param name="entities">Entities to add.</param>
    void AddRange(ICollection<TEntity> entities);

    /// <summary>
    /// Adds entities and commits.
    /// </summary>
    /// <param name="entities">Entities to add.</param>
    /// <returns>The added entities.</returns>
    ICollection<TEntity> AddRangeAndCommit(ICollection<TEntity> entities);

    #endregion

    #endregion

    #region Update

    #region Async

    /// <summary>
    /// Updates an entity.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Updates an entity and commits.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <returns>A task returning the updated entity.</returns>
    Task<TEntity> UpdateAndCommitAsync(TEntity entity);

    /// <summary>
    /// Updates entities.
    /// </summary>
    /// <param name="entities">Entities to update.</param>
    Task UpdateRangeAsync(ICollection<TEntity> entities);

    /// <summary>
    /// Updates entities and commits.
    /// </summary>
    /// <param name="entities">Entities to update.</param>
    /// <returns>A task returning the updated entities.</returns>
    Task<ICollection<TEntity>> UpdateRangeAndCommitAsync(ICollection<TEntity> entities);

    #endregion

    #region Sync

    /// <summary>
    /// Updates an entity.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    /// Updates an entity and commits.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    /// <returns>The updated entity.</returns>
    TEntity UpdateAndCommit(TEntity entity);

    /// <summary>
    /// Updates entities.
    /// </summary>
    /// <param name="entities">Entities to update.</param>
    void UpdateRange(ICollection<TEntity> entities);

    /// <summary>
    /// Updates entities and commits.
    /// </summary>
    /// <param name="entities">Entities to update.</param>
    /// <returns>The updated entities.</returns>
    ICollection<TEntity> UpdateRangeAndCommit(ICollection<TEntity> entities);

    #endregion

    #endregion

    #region Remove

    #region Async

    /// <summary>
    /// Removes an entity by id.
    /// </summary>
    /// <param name="id">Entity id.</param>
    Task RemoveAsync(long? id);

    /// <summary>
    /// Removes an entity by id and commits.
    /// </summary>
    /// <param name="id">Entity id.</param>
    Task RemoveAndCommitAsync(long? id);

    /// <summary>
    /// Removes entities by ids.
    /// </summary>
    /// <param name="ids">Entity ids.</param>
    Task RemoveRangeAsync(params long?[] ids);

    /// <summary>
    /// Removes entities by ids and commits.
    /// </summary>
    /// <param name="ids">Entity ids.</param>
    Task RemoveRangeAndCommitAsync(params long?[] ids);

    #endregion

    #region Sync

    /// <summary>
    /// Removes an entity by id.
    /// </summary>
    /// <param name="id">Entity id.</param>
    void Remove(long? id);

    /// <summary>
    /// Removes an entity by id and commits.
    /// </summary>
    /// <param name="id">Entity id.</param>
    void RemoveAndCommit(long? id);

    /// <summary>
    /// Removes entities by ids.
    /// </summary>
    /// <param name="ids">Entity ids.</param>
    void RemoveRange(params long?[] ids);

    /// <summary>
    /// Removes entities by ids and commits.
    /// </summary>
    /// <param name="ids">Entity ids.</param>
    void RemoveRangeAndCommit(params long?[] ids);

    #endregion

    #endregion

    #region Any

    #region Async

    /// <summary>
    /// Checks if any entity matches a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns><c>true</c> if any match; otherwise, <c>false</c>.</returns>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #region Sync

    /// <summary>
    /// Checks if any entity matches a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns><c>true</c> if any match; otherwise, <c>false</c>.</returns>
    bool Any(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #endregion

    #region Count

    #region Async

    /// <summary>
    /// Counts entities matching a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns>Total matching count.</returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #region Sync

    /// <summary>
    /// Counts entities matching a predicate.
    /// </summary>
    /// <param name="predicate">Filter expression.</param>
    /// <returns>Total matching count.</returns>
    int Count(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #endregion

    #region Detach

    /// <summary>
    /// Detaches an entity from tracking.
    /// </summary>
    /// <param name="entity">Entity to detach.</param>
    void Detach(TEntity entity);

    /// <summary>
    /// Detaches all tracked entities.
    /// </summary>
    void DetachAll();

    #endregion

    #region Change Tracking

    /// <summary>
    /// Disables change tracking.
    /// </summary>
    void DisableChangeTracker();

    /// <summary>
    /// Enables change tracking.
    /// </summary>
    void EnableChangeTracker();

    #endregion

    #region Commit

    #region Async

    /// <summary>
    /// Commits pending changes.
    /// </summary>
    /// <returns>Number of written entries.</returns>
    Task<int> CommitAsync();

    #endregion

    #region Sync

    /// <summary>
    /// Commits pending changes.
    /// </summary>
    /// <returns>Number of written entries.</returns>
    int Commit();

    #endregion

    #endregion
}
