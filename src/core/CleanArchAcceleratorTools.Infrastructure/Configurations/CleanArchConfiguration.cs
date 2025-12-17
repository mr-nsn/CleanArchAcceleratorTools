using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Infrastructure.UOW;
using CleanArchAcceleratorTools.Mediator.DomainNotification.Handler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchAcceleratorTools.Domain.Configurations;

/// <summary>
/// DI extensions for registering core CleanArch components.
/// </summary>
/// <remarks>
/// Adds mediator, domain notification handler, DbContext registrator, and unit of work
/// with scoped lifetimes. Call <see cref="AddCleanArchConfiguration(IServiceCollection)"/> at startup.
/// </remarks>
public static class CleanArchConfiguration
{
    /// <summary>
    /// Registers CleanArch services into the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance for chaining.</returns>
    /// <remarks>
    /// Mappings:
    /// - <see cref="ICleanArchMediator"/> → <see cref="CleanArchMediator"/>
    /// - <see cref="INotificationHandler{TNotification}"/> for <see cref="DomainNotificationEvent"/> → <see cref="DomainNotificationEventHandler"/>
    /// - <see cref="IDbContextRegistratorService"/> → <see cref="DbContextRegistratorService"/>
    /// - <see cref="IUnitOfWork"/> → <see cref="UnitOfWork"/>
    /// </remarks>
    public static IServiceCollection AddCleanArchConfiguration<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped<ICleanArchMediator, CleanArchMediator>();
        services.AddScoped<INotificationHandler<DomainNotificationEvent>, DomainNotificationEventHandler>();
        services.AddScoped<IDbContextRegistratorService<TContext>, DbContextRegistratorService<TContext>>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
