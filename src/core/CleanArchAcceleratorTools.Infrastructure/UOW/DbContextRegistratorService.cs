using Microsoft.EntityFrameworkCore;

namespace CleanArchAcceleratorTools.Infrastructure.UOW;

/// <summary>
/// Lightweight implementation of <see cref="IDbContextRegistratorService"/> backed by an <see cref="IUnitOfWork"/> and EF Core <see cref="IDbContextFactory{TContext}"/>.
/// </summary>
/// <remarks>
/// Registers contexts by key, creates new ones via factory, and reuses existing ones when available.
/// </remarks>
public class DbContextRegistratorService<TContext> : IDbContextRegistratorService<TContext> where TContext : DbContext
{
    /// <summary>
    /// Unit of work for context registration and retrieval.
    /// </summary>
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Factory to create new <see cref="DbContext"/> instances.
    /// </summary>
    private readonly IDbContextFactory<TContext> _dbFactory;

    /// <summary>
    /// Initializes the service.
    /// </summary>
    /// <param name="unitOfWork">Unit of work used to register and get contexts.</param>
    /// <param name="dbFactory">Factory for creating <see cref="DbContext"/> instances.</param>
    public DbContextRegistratorService(IUnitOfWork unitOfWork, IDbContextFactory<TContext> dbFactory)
    {
        _unitOfWork = unitOfWork;
        _dbFactory = dbFactory;
    }

    /// <inheritdoc />
    public bool Register(string key, TContext context)
    {
        return _unitOfWork.AddContextAsync(key, context);
    }

    /// <inheritdoc />
    public TContext Create(string key)
    {
        var context = _dbFactory.CreateDbContext();
        _unitOfWork.AddContextAsync(key, context);
        return context;
    }

    /// <inheritdoc />
    public TContext CreateOrReuse(string key)
    {
        return _unitOfWork.GetFirstDbContext(key) as TContext ?? Create(key);
    }
}
