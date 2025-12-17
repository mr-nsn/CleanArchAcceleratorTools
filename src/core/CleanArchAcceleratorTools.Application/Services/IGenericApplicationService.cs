using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Domain.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchAcceleratorTools.Application.Services;

/// <summary>
/// Compact application service contract for <typeparamref name="TEntity"/> and its view model <typeparamref name="TVEntityiewModel"/>.
/// </summary>
/// <typeparam name="TEntity">Domain entity type, derives from <see cref="Entity"/>.</typeparam>
/// <typeparam name="TVEntityiewModel">View model type representing the entity in the application layer.</typeparam>
/// <remarks>
/// Exposes query helpers (pagination, dynamic select/filter/sort), CRUD (sync/async), transactions, and notifications.
/// </remarks>
public interface IGenericApplicationService<TEntity, TVEntityiewModel> where TEntity : Entity
{
    #region Repository Methods    

    #endregion

    #region Repository Methods

    #region Get and Find

    #region Async

    /// <summary>
    /// Runs a paginated query using the given filter and returns view-model results.
    /// </summary>
    /// <param name="queryFilter">Filter with page, size, fields, and optional dynamic filter/sort.</param>
    /// <returns>Pagination result with metadata and view-model items.</returns>
    Task<PaginationResultViewModel<TVEntityiewModel>> SearchWithPaginationAsync(QueryFilterViewModel<TVEntityiewModel> queryFilter);

    /// <summary>
    /// Projects entities to the view model including only specified fields, with optional filter and sort.
    /// </summary>
    /// <param name="dynamicFilter">Optional dynamic filter.</param>
    /// <param name="dynamicSort">Optional dynamic sort.</param>
    /// <param name="fields">Field names to include.</param>
    /// <returns>Collection of view-model items.</returns>
    Task<ICollection<TVEntityiewModel>> DynamicSelectAsync(DynamicFilterViewModel<TVEntityiewModel>? dynamicFilter = null, DynamicSortViewModel<TVEntityiewModel>? dynamicSort = null, params string[] fields);

    /// <summary>
    /// Retrieves all entities mapped to the view model asynchronously.
    /// </summary>
    /// <returns>All view-model items.</returns>
    Task<ICollection<TVEntityiewModel>> GetAllAsync();

    /// <summary>
    /// Retrieves a view-model item by its identifier asynchronously.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>The view-model item if found; otherwise, null.</returns>
    Task<TVEntityiewModel?> GetByIdAsync(long? id);

    #endregion

    #region Sync

    /// <summary>
    /// Runs a paginated query using the given filter and returns view-model results.
    /// </summary>
    /// <param name="queryFilter">Filter with page, size, fields, and optional dynamic filter/sort.</param>
    /// <returns>Pagination result with metadata and view-model items.</returns>
    PaginationResultViewModel<TVEntityiewModel> SearchWithPagination(QueryFilterViewModel<TVEntityiewModel> queryFilter);

    /// <summary>
    /// Projects entities to the view model including only specified fields, with optional filter and sort.
    /// </summary>
    /// <param name="dynamicFilter">Optional dynamic filter.</param>
    /// <param name="dynamicSort">Optional dynamic sort.</param>
    /// <param name="fields">Field names to include.</param>
    /// <returns>Collection of view-model items.</returns>
    ICollection<TVEntityiewModel> DynamicSelect(DynamicFilterViewModel<TVEntityiewModel>? dynamicFilter = null, DynamicSortViewModel<TVEntityiewModel>? dynamicSort = null, params string[] fields);

    /// <summary>
    /// Retrieves all entities mapped to the view model.
    /// </summary>
    /// <returns>All view-model items.</returns>
    ICollection<TVEntityiewModel> GetAll();

    /// <summary>
    /// Retrieves a view-model item by its identifier.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>The view-model item if found; otherwise, null.</returns>
    TVEntityiewModel? GetById(long? id);

    #endregion

    #endregion

    #region Add

    #region Async

    /// <summary>
    /// Adds a view-model item asynchronously.
    /// </summary>
    /// <param name="entity">Item to add.</param>
    Task AddAsync(TVEntityiewModel entity);

    /// <summary>
    /// Adds a view-model item and commits asynchronously.
    /// </summary>
    /// <param name="entity">Item to add.</param>
    /// <returns>The persisted item.</returns>
    Task<TVEntityiewModel> AddAndCommitAsync(TVEntityiewModel entity);

    /// <summary>
    /// Adds multiple view-model items asynchronously.
    /// </summary>
    /// <param name="entities">Items to add.</param>
    Task AddRangeAsync(ICollection<TVEntityiewModel> entities);

    /// <summary>
    /// Adds multiple view-model items and commits asynchronously.
    /// </summary>
    /// <param name="entities">Items to add.</param>
    /// <returns>Persisted items.</returns>
    Task<ICollection<TVEntityiewModel>> AddRangeAndCommitAsync(ICollection<TVEntityiewModel> entities);

    #endregion

    #region Sync

    /// <summary>
    /// Adds a view-model item.
    /// </summary>
    /// <param name="entity">Item to add.</param>
    void Add(TVEntityiewModel entity);

    /// <summary>
    /// Adds a view-model item and commits.
    /// </summary>
    /// <param name="entity">Item to add.</param>
    /// <returns>The persisted item.</returns>
    TVEntityiewModel AddAndCommit(TVEntityiewModel entity);

    /// <summary>
    /// Adds multiple view-model items.
    /// </summary>
    /// <param name="entities">Items to add.</param>
    void AddRange(ICollection<TVEntityiewModel> entities);

    /// <summary>
    /// Adds multiple view-model items and commits.
    /// </summary>
    /// <param name="entities">Items to add.</param>
    /// <returns>Persisted items.</returns>
    ICollection<TVEntityiewModel> AddRangeAndCommit(ICollection<TVEntityiewModel> entities);

    #endregion

    #endregion

    #region Update

    #region Async

    /// <summary>
    /// Updates a view-model item asynchronously.
    /// </summary>
    /// <param name="entity">Item to update.</param>
    Task UpdateAsync(TVEntityiewModel entity);

    /// <summary>
    /// Updates a view-model item and commits asynchronously.
    /// </summary>
    /// <param name="entity">Item to update.</param>
    /// <returns>The persisted item.</returns>
    Task<TVEntityiewModel> UpdateAndCommitAsync(TVEntityiewModel entity);

    /// <summary>
    /// Updates multiple view-model items asynchronously.
    /// </summary>
    /// <param name="entities">Items to update.</param>
    Task UpdateRangeAsync(ICollection<TVEntityiewModel> entities);

    /// <summary>
    /// Updates multiple view-model items and commits asynchronously.
    /// </summary>
    /// <param name="entities">Items to update.</param>
    /// <returns>Persisted items.</returns>
    Task<ICollection<TVEntityiewModel>> UpdateRangeAndCommitAsync(ICollection<TVEntityiewModel> entities);

    #endregion

    #region Sync

    /// <summary>
    /// Updates a view-model item.
    /// </summary>
    /// <param name="entity">Item to update.</param>
    void Update(TVEntityiewModel entity);

    /// <summary>
    /// Updates a view-model item and commits.
    /// </summary>
    /// <param name="entity">Item to update.</param>
    /// <returns>The persisted item.</returns>
    TVEntityiewModel UpdateAndCommit(TVEntityiewModel entity);

    /// <summary>
    /// Updates multiple view-model items.
    /// </summary>
    /// <param name="entities">Items to update.</param>
    void UpdateRange(ICollection<TVEntityiewModel> entities);

    /// <summary>
    /// Updates multiple view-model items and commits.
    /// </summary>
    /// <param name="entities">Items to update.</param>
    /// <returns>Persisted items.</returns>
    ICollection<TVEntityiewModel> UpdateRangeAndCommit(ICollection<TVEntityiewModel> entities);

    #endregion

    #endregion

    #region Remove

    #region Async

    /// <summary>
    /// Removes an item by identifier asynchronously.
    /// </summary>
    /// <param name="id">Item identifier.</param>
    Task RemoveAsync(long? id);

    /// <summary>
    /// Removes an item by identifier and commits asynchronously.
    /// </summary>
    /// <param name="id">Item identifier.</param>
    Task RemoveAndCommitAsync(long? id);

    /// <summary>
    /// Removes multiple items by identifiers asynchronously.
    /// </summary>
    /// <param name="ids">Item identifiers.</param>
    Task RemoveRangeAsync(params long?[] ids);

    /// <summary>
    /// Removes multiple items by identifiers and commits asynchronously.
    /// </summary>
    /// <param name="ids">Item identifiers.</param>
    Task RemoveRangeAndCommitAsync(params long?[] ids);

    #endregion

    #region Sync

    /// <summary>
    /// Removes an item by identifier.
    /// </summary>
    /// <param name="id">Item identifier.</param>
    void Remove(long? id);

    /// <summary>
    /// Removes an item by identifier and commits.
    /// </summary>
    /// <param name="id">Item identifier.</param>
    void RemoveAndCommit(long? id);

    /// <summary>
    /// Removes multiple items by identifiers.
    /// </summary>
    /// <param name="ids">Item identifiers.</param>
    void RemoveRange(params long?[] ids);

    /// <summary>
    /// Removes multiple items by identifiers and commits.
    /// </summary>
    /// <param name="ids">Item identifiers.</param>
    void RemoveRangeAndCommit(params long?[] ids);

    #endregion

    #endregion

    #endregion

    #region Transactions

    /// <summary>
    /// Begins a database transaction.
    /// </summary>
    /// <returns>Transaction object for controlling commit/rollback.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Commits changes across contexts.
    /// </summary>
    /// <param name="ignoreNotifications">If true, skips notification checks before committing.</param>
    /// <returns>Total entries written to the database.</returns>
    Task<int> CommitAsync(bool ignoreNotifications = false);

    /// <summary>
    /// Commits changes for contexts under a specific key.
    /// </summary>
    /// <param name="key">Logical key identifying the contexts to commit.</param>
    /// <param name="ignoreNotifications">If true, skips notification checks before committing.</param>
    /// <returns>Total entries written to the database for the specified key.</returns>
    Task<int> CommitAsync(string key, bool ignoreNotifications = false);

    /// <summary>
    /// Discards and detaches all registered DbContext instances.
    /// </summary>
    Task DiscartRegisteredContextAsync();

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