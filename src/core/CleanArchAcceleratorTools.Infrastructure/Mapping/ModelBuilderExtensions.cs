using CleanArchAcceleratorTools.Domain.Interfaces;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using CleanArchAcceleratorTools.Domain.Messages;
using CleanArchAcceleratorTools.Domain.Util;

namespace CleanArchAcceleratorTools.Infrastructure.Mapping
{
    /// <summary>
    /// EF Core <see cref="ModelBuilder"/> extensions to apply entity configurations.
    /// </summary>
    /// <remarks>
    /// - <see cref="AddConfiguration{TEntity}(ModelBuilder, EntityTypeConfiguration{TEntity})"/> applies a single, typed configuration.
    /// - <see cref="RegisterModelsMapping(ModelBuilder, System.Collections.Generic.Dictionary{System.Type, IEntityTypeConfiguration})"/> applies multiple configurations via reflection.
    /// </remarks>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Applies a specific entity configuration to the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <typeparam name="TEntity">Entity type to configure. Must implement <see cref="IAggregateRoot"/>.</typeparam>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to configure.</param>
        /// <param name="configuration">The configuration to apply.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="modelBuilder"/> or <paramref name="configuration"/> is null.</exception>
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity> configuration) where TEntity : class, IAggregateRoot
        {
            if (modelBuilder is null) throw new ArgumentNullException(nameof(modelBuilder));
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            configuration.Map(modelBuilder.Entity<TEntity>());
        }

        /// <summary>
        /// Registers multiple entity configurations with the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to configure.</param>
        /// <param name="mappings">Dictionary mapping entity types to their <see cref="IEntityTypeConfiguration"/> implementations.</param>
        /// <remarks>
        /// Uses reflection to invoke <see cref="AddConfiguration{TEntity}(ModelBuilder, EntityTypeConfiguration{TEntity})"/> for each entry.
        /// Also ignores <see cref="ValidationResult"/> and <see cref="ValidationFailure"/> types.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="modelBuilder"/> or <paramref name="mappings"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when a mapping has an incompatible configuration type.</exception>
        public static void RegisterModelsMapping(this ModelBuilder modelBuilder, Dictionary<Type, IEntityTypeConfiguration> mappings)
        {
            if (modelBuilder is null) throw new ArgumentNullException(nameof(modelBuilder));
            if (mappings is null) throw new ArgumentNullException(nameof(mappings));

            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<ValidationFailure>();

            foreach (var mapping in mappings)
            {
                var entityType = mapping.Key;
                var configuration = mapping.Value;

                // Ensure configuration is of the expected generic type: EntityTypeConfiguration<TEntity>
                var expectedGeneric = typeof(EntityTypeConfiguration<>).MakeGenericType(entityType);
                if (!expectedGeneric.IsAssignableFrom(configuration.GetType()))
                {
                    throw new ArgumentException(
                        DomainMessages.ConfigurationTypeNotAssignableForEntity.ToFormat(
                            configuration.GetType().FullName,
                            entityType.FullName,
                            expectedGeneric.FullName));
                }

                var genericMethod = typeof(ModelBuilderExtensions).GetMethod(nameof(ModelBuilderExtensions.AddConfiguration));
                var method = genericMethod!.MakeGenericMethod(entityType);
                method!.Invoke(null, new object[] { modelBuilder, configuration });
            }
        }
    }
}