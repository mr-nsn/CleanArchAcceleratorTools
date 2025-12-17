using CleanArchAcceleratorTools.Domain.Interfaces.Repository;
using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Mediator.DomainNotification.Handler;
using System.Linq.Expressions;
using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Models.Validators;
using CleanArchAcceleratorTools.Domain.Exceptions;
using FluentValidation;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Domain.Services;

/// <summary>
/// <inheritdoc cref="IGenericDomainService{TEntity}"/>
/// </summary>
/// <typeparam name="TEntity"><inheritdoc cref="IGenericDomainService{TEntity}" path="/typeparam[@name='TEntity']"/></typeparam>
/// <remarks>
/// - Exposes synchronous and asynchronous CRUD operations and query helpers.
/// - Publishes domain notifications using <see cref="ICleanArchMediator"/> and logs them via <see cref="IApplicationLogger"/>.
/// - Delegates change tracking and detaching operations to the underlying repository.
/// </remarks>
public class GenericDomainService<TEntity> : IGenericDomainService<TEntity> where TEntity : Entity
{
    /// <summary>Mediator used to send requests and publish notifications.</summary>
    protected readonly ICleanArchMediator _mediator;

    /// <summary>Notification handler storing in-memory domain notifications.</summary>
    protected readonly DomainNotificationEventHandler _notifications;

    /// <summary>Generic repository for data access and persistence operations.</summary>
    protected readonly IGenericRepository<TEntity> _repository;

    /// <summary>Application logger for structured notification logging.</summary>
    protected readonly IApplicationLogger _applicationLogger;

    /// <summary>Fluent validator of the entity.</summary>
    protected readonly IValidator<TEntity> _entityValidator;

    /// <summary>
    /// Initializes a new instance of the domain service.
    /// </summary>
    /// <param name="mediator">Mediator instance for notifications.</param>
    /// <param name="notifications">Notification handler (cast to <see cref="DomainNotificationEventHandler"/>).</param>
    /// <param name="repository">Generic repository for <typeparamref name="TEntity"/>.</param>
    /// <param name="applicationLogger">Application logger for notifications.</param>
    public GenericDomainService(
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> notifications,
        IGenericRepository<TEntity> repository,
        IValidator<TEntity> entityValidator,
        IApplicationLogger applicationLogger)
    {
        _mediator = mediator;
        _notifications = (DomainNotificationEventHandler)notifications;
        _repository = repository;
        _entityValidator = entityValidator;
        _applicationLogger = applicationLogger;
    }

    #region Repository Methods

    #region Get and Find

    #region Async

    /// <inheritdoc />
    public Task<PaginationResult<TEntity>> SearchWithPaginationAsync(QueryFilter<TEntity> queryFilter)
    {
        if (!queryFilter.Validate(new QueryFilterValidator<TEntity>())) throw new DomainException(queryFilter.ValidationResult);
        return _repository.SearchWithPaginationAsync(queryFilter);
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntity>> DynamicSelectAsync(DynamicFilter<TEntity>? dynamicFilter = null, DynamicSort<TEntity>? dynamicSort = null, params string[] fields)
    {
        return await _repository.DynamicSelectAsync(dynamicFilter, dynamicSort, fields);
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetByIdAsync(long? id)
    {
        return await _repository.GetByIdAsync(id);
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public PaginationResult<TEntity> SearchWithPagination(QueryFilter<TEntity> queryFilter)
    {
        return _repository.SearchWithPagination(queryFilter);
    }

    /// <inheritdoc />
    public ICollection<TEntity> DynamicSelect(DynamicFilter<TEntity>? dynamicFilter = null, DynamicSort<TEntity>? dynamicSort = null, params string[] fields)
    {
        return _repository.DynamicSelect(dynamicFilter, dynamicSort, fields);
    }

    /// <inheritdoc />
    public ICollection<TEntity> GetAll()
    {
        return _repository.GetAll();
    }

    /// <inheritdoc />
    public TEntity? GetById(long? id)
    {
        return _repository.GetById(id);
    }

    /// <inheritdoc />
    public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
    {
        return _repository.FindAll(predicate);
    }

    /// <inheritdoc />
    public TEntity? FindFirst(Expression<Func<TEntity, bool>> predicate)
    {
        return _repository.FindFirst(predicate);
    }

    #endregion

    #endregion

    #region Add

    #region Async

    /// <inheritdoc />
    public async Task AddAsync(TEntity entity)
    {
        if (!ValidateEntity(entity)) return;
        await _repository.AddAsync(entity);
    }

    /// <inheritdoc />
    public async Task<TEntity> AddAndCommitAsync(TEntity entity)
    {
        if (!ValidateEntity(entity)) return entity;
        return await _repository.AddAndCommitAsync(entity);
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return;
        await _repository.AddRangeAsync(entities);
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntity>> AddRangeAndCommitAsync(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return entities;
        return await _repository.AddRangeAndCommitAsync(entities);
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public void Add(TEntity entity)
    {
        if (!ValidateEntity(entity)) return;
        _repository.Add(entity);
    }

    /// <inheritdoc />
    public TEntity AddAndCommit(TEntity entity)
    {
        if (!ValidateEntity(entity)) return entity;
        return _repository.AddAndCommit(entity);
    }

    /// <inheritdoc />
    public void AddRange(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return;
        _repository.AddRange(entities);
    }

    /// <inheritdoc />
    public ICollection<TEntity> AddRangeAndCommit(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return entities;
        return _repository.AddRangeAndCommit(entities);
    }

    #endregion

    #endregion

    #region Update

    #region Async

    /// <inheritdoc />
    public async Task UpdateAsync(TEntity entity)
    {
        if (!ValidateEntity(entity)) return;
        await _repository.UpdateAsync(entity);
    }

    /// <inheritdoc />
    public async Task<TEntity> UpdateAndCommitAsync(TEntity entity)
    {
        if (!ValidateEntity(entity)) return entity;
        return await _repository.UpdateAndCommitAsync(entity);
    }

    /// <inheritdoc />
    public async Task UpdateRangeAsync(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return;
        await _repository.UpdateRangeAsync(entities);
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntity>> UpdateRangeAndCommitAsync(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return entities;
        return await _repository.UpdateRangeAndCommitAsync(entities);
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public void Update(TEntity entity)
    {
        if (!ValidateEntity(entity)) return;
        _repository.Update(entity);
    }

    /// <inheritdoc />
    public TEntity UpdateAndCommit(TEntity entity)
    {
        if (!ValidateEntity(entity)) return entity;
        return _repository.UpdateAndCommit(entity);
    }

    /// <inheritdoc />
    public void UpdateRange(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return;
        _repository.UpdateRange(entities);
    }

    /// <inheritdoc />
    public ICollection<TEntity> UpdateRangeAndCommit(ICollection<TEntity> entities)
    {
        if (!ValidateEntities(entities)) return entities;
        return _repository.UpdateRangeAndCommit(entities);
    }

    #endregion

    #endregion

    #region Remove

    #region Async

    /// <inheritdoc />
    public async Task RemoveAsync(long? id)
    {
        await _repository.RemoveAsync(id);
    }

    /// <inheritdoc />
    public async Task RemoveAndCommitAsync(long? id)
    {
        await _repository.RemoveAndCommitAsync(id);
    }

    /// <inheritdoc />
    public async Task RemoveRangeAsync(params long?[] ids)
    {
        await _repository.RemoveRangeAsync(ids);
    }

    /// <inheritdoc />
    public async Task RemoveRangeAndCommitAsync(params long?[] ids)
    {
        await _repository.RemoveRangeAndCommitAsync(ids);
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public void Remove(long? id)
    {
        _repository.Remove(id);
    }

    /// <inheritdoc />
    public void RemoveAndCommit(long? id)
    {
        _repository.RemoveAndCommit(id);
    }

    /// <inheritdoc />
    public void RemoveRange(params long?[] ids)
    {
        _repository.RemoveRange(ids);
    }

    /// <inheritdoc />
    public void RemoveRangeAndCommit(params long?[] ids)
    {
        _repository.RemoveRangeAndCommit(ids);
    }

    #endregion

    #endregion

    #endregion

    #region Notifications

    /// <inheritdoc />
    public async Task RaiseDatedNotificationAsync(DomainNotificationType type, string key, string message)
    {
        var timestamp = DateTime.Now.ToString(DomainMessages.DatedNotificationTimestampFormat);
        var datedMessage = DomainMessages.DatedNotificationTemplate.ToFormat(timestamp, message);

        var @event = new DomainNotificationEventBuilder(type, key)
            .WithValue(datedMessage)
            .Build();

        await LogNotificationAsync(@event.Type, @event.Value);
        await _mediator.PublishAsync(@event);
    }

    /// <inheritdoc />
    public async Task RaiseNotificationAsync(DomainNotificationType type, string key, string message)
    {
        var @event = new DomainNotificationEventBuilder(type, key)
            .WithValue(message)
            .Build();

        await LogNotificationAsync(@event.Type, @event.Value);
        await _mediator.PublishAsync(@event);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Logs a notification using the appropriate log level based on <paramref name="type"/>.
    /// </summary>
    /// <param name="type">Notification type/severity.</param>
    /// <param name="message">Message to log.</param>
    private async Task LogNotificationAsync(DomainNotificationType type, string message)
    {
        switch (type)
        {
            case DomainNotificationType.Error:
                await _applicationLogger.LogErrorAsync(DomainMessages.LogLabelError, message);
                break;
            case DomainNotificationType.Warning:
                await _applicationLogger.LogWarningAsync(DomainMessages.LogLabelWarning, message);
                break;
            case DomainNotificationType.Information:
                await _applicationLogger.LogInformationAsync(DomainMessages.LogLabelInformation, message);
                break;
            case DomainNotificationType.Trace:
                await _applicationLogger.LogTraceAsync(DomainMessages.LogLabelTrace, message);
                break;
            case DomainNotificationType.Debug:
                await _applicationLogger.LogDebugAsync(DomainMessages.LogLabelDebug, message);
                break;
        }
    }

    private bool ValidateEntities(ICollection<TEntity> entities)
    {
        var validEntities = new List<bool>();

        foreach (var entity in entities)
            validEntities.Add(ValidateEntity(entity));

        return validEntities.All(x => x);
    }

    private bool ValidateEntity(TEntity entity)
    {
        var entityIsValid = true;

        if (!entity.Validate(_entityValidator))
        {
            entityIsValid = false;

            foreach (var error in entity.ValidationResult.Errors)
                RaiseDatedNotificationAsync(DomainNotificationType.Error, DomainMessages.EntityValidationKey, error.ErrorMessage).GetAwaiter().GetResult();
        }

        return entityIsValid;
    }

    #endregion
}