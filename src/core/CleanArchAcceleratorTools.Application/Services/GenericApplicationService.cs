using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Interfaces.Repository;
using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Services;
using CleanArchAcceleratorTools.Domain.Util;
using CleanArchAcceleratorTools.Infrastructure.UOW;
using CleanArchAcceleratorTools.Mediator.DomainNotification.Handler;
using Mapster;
using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchAcceleratorTools.Application.Services;

/// <summary>
/// Application service that adapts view models to domain entities, delegates operations to the domain layer,
/// coordinates transactions via unit of work, and publishes domain notifications.
/// </summary>
/// <typeparam name="TEntity"><inheritdoc cref="IGenericApplicationService{TEntity, TViewModelEntity}" path="/typeparam[@name='TEntity']"/></typeparam>
/// <typeparam name="TEntityViewModel"><inheritdoc cref="IGenericApplicationService{TEntity, TViewModelEntity}" path="/typeparam[@name='TViewModelEntity']"/></typeparam>
/// <remarks>
/// Uses Mapster for mapping, <see cref="ICleanArchMediator"/> for notifications, and <see cref="IUnitOfWork"/> for commits.
/// Most member documentation is inherited from <see cref="IGenericApplicationService{TEntity, TViewModelEntity}"/>.
/// </remarks>
public class GenericApplicationService<TEntity, TEntityViewModel> : IGenericApplicationService<TEntity, TEntityViewModel> where TEntity : Entity
{
    protected readonly ICleanArchMediator _mediator;
    protected readonly DomainNotificationEventHandler _notifications;
    protected readonly IGenericDomainService<TEntity> _domainService;

    protected readonly IUnitOfWork _unitOfWork;
    protected readonly IGenericRepository<TEntity> _repository;

    protected readonly IApplicationLogger _applicationLogger;

    /// <summary>
    /// Initializes a new instance of the application service.
    /// </summary>
    /// <param name="repository">Generic repository for <typeparamref name="TEntity"/>.</param>
    /// <param name="domainService">Domain service for business operations.</param>
    /// <param name="unitOfWork">Unit of work for transaction management.</param>
    /// <param name="mediator">Mediator for notification publishing.</param>
    /// <param name="notifications">Notification handler (cast to <see cref="DomainNotificationEventHandler"/>).</param>
    /// <param name="applicationLogger">Application logger for notifications.</param>
    public GenericApplicationService(
        IGenericRepository<TEntity> repository,
        IGenericDomainService<TEntity> domainService,
        IUnitOfWork unitOfWork,
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> notifications,
        IApplicationLogger applicationLogger)
    {
        _domainService = domainService;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _notifications = (DomainNotificationEventHandler)notifications;
        _applicationLogger = applicationLogger;
    }

    #region Repository Methods

    #region Get and Find

    #region Async

    /// <inheritdoc />
    public async Task<PaginationResultViewModel<TEntityViewModel>> SearchWithPaginationAsync(QueryFilterViewModel<TEntityViewModel> queryFilter)
    {
        var values = await _domainService.SearchWithPaginationAsync(queryFilter.Adapt<QueryFilter<TEntity>>());
        return values.Adapt<PaginationResultViewModel<TEntityViewModel>>();
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntityViewModel>> DynamicSelectAsync(DynamicFilterViewModel<TEntityViewModel>? dynamicFilter = null, DynamicSortViewModel<TEntityViewModel>? dynamicSort = null, params string[] fields)
    {
        var result = await _domainService.DynamicSelectAsync(dynamicFilter.Adapt<DynamicFilter<TEntity>>(), dynamicSort.Adapt<DynamicSort<TEntity>>(), fields);
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntityViewModel>> GetAllAsync()
    {
        var result = await _domainService.GetAllAsync();
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    /// <inheritdoc />
    public async Task<TEntityViewModel?> GetByIdAsync(long? id)
    {
        var result = await _domainService.GetByIdAsync(id);
        return result.Adapt<TEntityViewModel>();
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public PaginationResultViewModel<TEntityViewModel> SearchWithPagination(QueryFilterViewModel<TEntityViewModel> queryFilter)
    {
        var result = _domainService.SearchWithPagination(queryFilter.Adapt<QueryFilter<TEntity>>());
        return result.Adapt<PaginationResultViewModel<TEntityViewModel>>();
    }

    /// <inheritdoc />
    public ICollection<TEntityViewModel> DynamicSelect(DynamicFilterViewModel<TEntityViewModel>? dynamicFilter = null, DynamicSortViewModel<TEntityViewModel>? dynamicSort = null, params string[] fields)
    {
        var result = _domainService.DynamicSelect(dynamicFilter.Adapt<DynamicFilter<TEntity>>(), dynamicSort.Adapt<DynamicSort<TEntity>>(), fields);
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    /// <inheritdoc />
    public ICollection<TEntityViewModel> GetAll()
    {
        var result = _domainService.GetAll();
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    /// <inheritdoc />
    public TEntityViewModel? GetById(long? id)
    {
        var result = _domainService.GetById(id);
        return result.Adapt<TEntityViewModel>();
    }

    #endregion

    #endregion

    #region Add

    #region Async

    /// <inheritdoc />
    public async Task AddAsync(TEntityViewModel entity)
    {
        await _domainService.AddAsync(entity.Adapt<TEntity>());
    }

    /// <inheritdoc />
    public async Task<TEntityViewModel> AddAndCommitAsync(TEntityViewModel entity)
    {
        var result = await _domainService.AddAndCommitAsync(entity.Adapt<TEntity>());
        return result.Adapt<TEntityViewModel>();
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(ICollection<TEntityViewModel> entities)
    {
        await _domainService.AddRangeAsync(entities.Adapt<ICollection<TEntity>>());
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntityViewModel>> AddRangeAndCommitAsync(ICollection<TEntityViewModel> entities)
    {
        var result = await _domainService.AddRangeAndCommitAsync(entities.Adapt<ICollection<TEntity>>());
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public void Add(TEntityViewModel entity)
    {
        _domainService.Add(entity.Adapt<TEntity>());
    }

    /// <inheritdoc />
    public TEntityViewModel AddAndCommit(TEntityViewModel entity)
    {
        var result = _domainService.AddAndCommit(entity.Adapt<TEntity>());
        return result.Adapt<TEntityViewModel>();
    }

    /// <inheritdoc />
    public void AddRange(ICollection<TEntityViewModel> entities)
    {
        _domainService.AddRange(entities.Adapt<ICollection<TEntity>>());
    }

    /// <inheritdoc />
    public ICollection<TEntityViewModel> AddRangeAndCommit(ICollection<TEntityViewModel> entities)
    {
        var result = _domainService.AddRangeAndCommit(entities.Adapt<ICollection<TEntity>>());
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    #endregion

    #endregion

    #region Update

    #region Async

    /// <inheritdoc />
    public async Task UpdateAsync(TEntityViewModel entity)
    {
        await _domainService.UpdateAsync(entity.Adapt<TEntity>());
    }

    /// <inheritdoc />
    public async Task<TEntityViewModel> UpdateAndCommitAsync(TEntityViewModel entity)
    {
        var result = await _domainService.UpdateAndCommitAsync(entity.Adapt<TEntity>());
        return result.Adapt<TEntityViewModel>();
    }

    /// <inheritdoc />
    public async Task UpdateRangeAsync(ICollection<TEntityViewModel> entities)
    {
        await _domainService.UpdateRangeAsync(entities.Adapt<ICollection<TEntity>>());
    }

    /// <inheritdoc />
    public async Task<ICollection<TEntityViewModel>> UpdateRangeAndCommitAsync(ICollection<TEntityViewModel> entities)
    {
        var result = await _domainService.UpdateRangeAndCommitAsync(entities.Adapt<ICollection<TEntity>>());
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public void Update(TEntityViewModel entity)
    {
        _domainService.Update(entity.Adapt<TEntity>());
    }

    /// <inheritdoc />
    public TEntityViewModel UpdateAndCommit(TEntityViewModel entity)
    {
        var result = _domainService.UpdateAndCommit(entity.Adapt<TEntity>());
        return result.Adapt<TEntityViewModel>();
    }

    /// <inheritdoc />
    public void UpdateRange(ICollection<TEntityViewModel> entities)
    {
        _domainService.UpdateRange(entities.Adapt<ICollection<TEntity>>());
    }

    /// <inheritdoc />
    public ICollection<TEntityViewModel> UpdateRangeAndCommit(ICollection<TEntityViewModel> entities)
    {
        var result = _domainService.UpdateRangeAndCommit(entities.Adapt<ICollection<TEntity>>());
        return result.Adapt<ICollection<TEntityViewModel>>();
    }

    #endregion

    #endregion

    #region Remove

    #region Async

    /// <inheritdoc />
    public async Task RemoveAsync(long? id)
    {
        await _domainService.RemoveAsync(id);
    }

    /// <inheritdoc />
    public async Task RemoveAndCommitAsync(long? id)
    {
        await _domainService.RemoveAndCommitAsync(id);
    }

    /// <inheritdoc />
    public async Task RemoveRangeAsync(params long?[] ids)
    {
        await _domainService.RemoveRangeAsync(ids);
    }

    /// <inheritdoc />
    public async Task RemoveRangeAndCommitAsync(params long?[] ids)
    {
        await _domainService.RemoveRangeAndCommitAsync(ids);
    }

    #endregion

    #region Sync

    /// <inheritdoc />
    public void Remove(long? id)
    {
        _domainService.Remove(id);
    }

    /// <inheritdoc />
    public void RemoveAndCommit(long? id)
    {
        _domainService.RemoveAndCommit(id);
    }

    /// <inheritdoc />
    public void RemoveRange(params long?[] ids)
    {
        _domainService.RemoveRange(ids);
    }

    /// <inheritdoc />
    public void RemoveRangeAndCommit(params long?[] ids)
    {
        _domainService.RemoveRangeAndCommit(ids);
    }

    #endregion

    #endregion

    #endregion

    #region Transactions

    /// <inheritdoc />
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _unitOfWork.BeginTransactionAsync();
    }

    /// <inheritdoc />
    public async Task<int> CommitAsync(bool ignoreNotifications = false)
    {
        if (!IsValidOperation() && !ignoreNotifications)
        {
            await RaiseNotificationAsync(DomainNotificationType.Error, Guid.NewGuid().ToString(), DomainMessages.ErrorPersistingData);
            return 0;
        }

        return await _unitOfWork.CommitAsync();
    }

    /// <inheritdoc />
    public async Task<int> CommitAsync(string key, bool ignoreNotifications = false)
    {
        if (!IsValidOperation() && !ignoreNotifications)
        {
            await RaiseNotificationAsync(DomainNotificationType.Error, Guid.NewGuid().ToString(), DomainMessages.ErrorPersistingData);
            return 0;
        }

        return await _unitOfWork.CommitAsync(key);
    }

    /// <inheritdoc />
    public async Task DiscartRegisteredContextAsync()
    {
        await _unitOfWork.DiscartAllDbContextAsync();
    }

    private bool IsValidOperation()
    {
        return !_notifications.HasError();
    }

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

    /// <summary>
    /// Logs a notification using the appropriate log level based on <paramref name="type"/>.
    /// </summary>
    /// <param name="type">Notification severity used to choose the log level.</param>
    /// <param name="message">Notification message to log.</param>
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

    #endregion
}