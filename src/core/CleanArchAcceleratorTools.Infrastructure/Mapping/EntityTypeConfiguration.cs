using CleanArchAcceleratorTools.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchAcceleratorTools.Infrastructure.Mapping;

/// <summary>
/// Base EF Core configuration for aggregate root entities.
/// </summary>
/// <typeparam name="TEntity">Entity type to configure; must implement <see cref="IAggregateRoot"/>.</typeparam>
/// <remarks>
/// Centralizes mapping for an entity (properties, keys/indexes, relationships, table/column names, converters).
/// Implement <see cref="Map(EntityTypeBuilder{TEntity})"/> in derived classes and invoke from your DbContext.
/// </remarks>
public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration where TEntity : class, IAggregateRoot
{
    /// <summary>
    /// Configures EF Core mapping for <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="builder">EF Core builder used to configure the entity.</param>
    public abstract void Map(EntityTypeBuilder<TEntity> builder);
}
