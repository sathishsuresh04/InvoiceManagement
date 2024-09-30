using InvoiceManagement.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManagement.Application.Abstractions.Data;

public interface IDbContext
{
    /// <summary>
    ///     Gets the database set for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The database set for the specified entity type.</returns>
    DbSet<TEntity> Set<TEntity>()
    where TEntity : Entity;

    /// <summary>
    ///     Gets the entity with the specified identifier.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="id">The entity identifier.</param>
    /// <returns>The <typeparamref name="TEntity" /> with the specified identifier if it exists, otherwise null.</returns>
    Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
    where TEntity : Entity;

    /// <summary>
    ///     Inserts the specified entity into the database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be inserted into the database.</param>
    void Insert<TEntity>(TEntity entity)
    where TEntity : Entity;

    /// <summary>
    ///     Inserts the specified entities into the database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entities">The entities to be inserted into the database.</param>
    void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
    where TEntity : Entity;

    /// <summary>
    ///     Removes the specified entity from the database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be removed from the database.</param>
    void Remove<TEntity>(TEntity entity)
    where TEntity : Entity;
}
