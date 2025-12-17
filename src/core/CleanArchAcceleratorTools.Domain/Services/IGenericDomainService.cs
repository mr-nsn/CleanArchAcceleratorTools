using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Domain.Models;

namespace CleanArchAcceleratorTools.Domain.Services;

/// <summary>
/// Compact domain service contract for repository operations, query helpers, change tracking, and notifications.
/// </summary>
/// <typeparam name="TEntity">Entity type deriving from <see cref="Entity"/>.</typeparam>
public interface IGenericDomainService<TEntity> where TEntity : Entity
{
    #region Repository Methods

    #region Get and Find

    #region Async

    /// <summary>
    /// Executes a paginated search with the provided filter.
    /// </summary>
    /// <param name="queryFilter">Pagination, fields, dynamic filter, and sort.</param>
    /// <returns>Paginated result with metadata and items.</returns>
    Task<PaginationResult<TEntity>> SearchWithPaginationAsync(QueryFilter<TEntity> queryFilter);

    /// <summary>
    /// Projects entities to selected fields with optional filter and sort.
    /// </summary>
    /// <param name="dynamicFilter">Optional dynamic filter.</param>
    /// <param name="dynamicSort">Optional dynamic sort.</param>
    /// <param name="fields">Field names to include.</param>
    /// <returns>Projected entities.</returns>
    Task<ICollection<TEntity>> DynamicSelectAsync(DynamicFilter<TEntity>? dynamicFilter = null, DynamicSort<TEntity>? dynamicSort = null, params string[] fields);

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>All entities.</returns>
    Task<ICollection<TEntity>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity by id.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<TEntity?> GetByIdAsync(long? id);

    #endregion

    #region Sync

    /// <summary>
    /// Executes a paginated search with the provided filter.
    /// </summary>
    /// <param name="queryFilter">Pagination, fields, dynamic filter, and sort.</param>
    /// <returns>Paginated result with metadata and items.</returns>
    PaginationResult<TEntity> SearchWithPagination(QueryFilter<TEntity> queryFilter);

    /// <summary>
    /// Projects entities to selected fields with optional filter and sort.
    /// </summary>
    /// <param name="dynamicFilter">Optional dynamic filter.</param>
    /// <param name="dynamicSort">Optional dynamic sort.</param>
    /// <param name="fields">Field names to include.</param>
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
    /// <param name="id">Entity identifier.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    TEntity? GetById(long? id);

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
    /// <returns>Persisted entity.</returns>
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
    /// <returns>Persisted entities.</returns>
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
    /// <returns>Persisted entity.</returns>
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
    /// <returns>Persisted entities.</returns>
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
    /// <returns>Persisted entity.</returns>
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
    /// <returns>Persisted entities.</returns>
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
    /// <returns>Persisted entity.</returns>
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
    /// <returns>Persisted entities.</returns>
    ICollection<TEntity> UpdateRangeAndCommit(ICollection<TEntity> entities);

    #endregion

    #endregion

    #region Remove

    #region Async

    /// <summary>
    /// Removes an entity by id.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    Task RemoveAsync(long? id);

    /// <summary>
    /// Removes an entity by id and commits.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    Task RemoveAndCommitAsync(long? id);

    /// <summary>
    /// Removes multiple entities by ids.
    /// </summary>
    /// <param name="ids">Entity identifiers.</param>
    Task RemoveRangeAsync(params long?[] ids);

    /// <summary>
    /// Removes multiple entities by ids and commits.
    /// </summary>
    /// <param name="ids">Entity identifiers.</param>
    Task RemoveRangeAndCommitAsync(params long?[] ids);

    #endregion

    #region Sync

    /// <summary>
    /// Removes an entity by id.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    void Remove(long? id);

    /// <summary>
    /// Removes an entity by id and commits.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    void RemoveAndCommit(long? id);

    /// <summary>
    /// Removes multiple entities by ids.
    /// </summary>
    /// <param name="ids">Entity identifiers.</param>
    void RemoveRange(params long?[] ids);

    /// <summary>
    /// Removes multiple entities by ids and commits.
    /// </summary>
    /// <param name="ids">Entity identifiers.</param>
    void RemoveRangeAndCommit(params long?[] ids);

    #endregion

    #endregion

    #endregion

    #region Notifications

    /// <summary>
    /// Publishes a dated domain notification (message prefixed with timestamp).
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="key">Notification key/category.</param>
    /// <param name="message">Notification message.</param>
    Task RaiseDatedNotificationAsync(DomainNotificationType type, string key, string message);

    /// <summary>
    /// Publishes a domain notification.
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="key">Notification key/category.</param>
    /// <param name="message">Notification message.</param>
    Task RaiseNotificationAsync(DomainNotificationType type, string key, string message);

    #endregion
}
