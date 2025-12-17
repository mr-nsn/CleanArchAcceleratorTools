using Microsoft.EntityFrameworkCore;

namespace CleanArchAcceleratorTools.Infrastructure.UOW;

/// <summary>
/// Contract to register and retrieve <see cref="DbContext"/> instances by a logical key, enabling reuse or creation on demand.
/// </summary>
/// <remarks>
/// Typical use: register an existing context, create a new one, or reuse a previously registered instance.
/// Implementations may rely on caching or factories to manage lifetimes.
/// </remarks>
public interface IDbContextRegistratorService<TContext> where TContext : DbContext
{
    /// <summary>
    /// Registers an existing <see cref="DbContext"/> under the given key.
    /// </summary>
    /// <param name="key">Logical key associated with the context.</param>
    /// <param name="context">The <see cref="DbContext"/> instance to register.</param>
    /// <returns><c>true</c> if registration succeeds; otherwise, <c>false</c>.</returns>
    bool Register(string key, TContext context);

    /// <summary>
    /// Creates a new <see cref="DbContext"/> for the specified key.
    /// </summary>
    /// <param name="key">Logical key used to instantiate the context.</param>
    /// <returns>A newly created <see cref="DbContext"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the key is unknown.</exception>
    TContext Create(string key);

    /// <summary>
    /// Creates or reuses a <see cref="DbContext"/> for the specified key.
    /// </summary>
    /// <param name="key">Logical key for an existing or creatable context.</param>
    /// <returns>An existing registered <see cref="DbContext"/> if available; otherwise, a new instance.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the key is unknown.</exception>
    TContext CreateOrReuse(string key);
}
