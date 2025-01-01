using System.Linq.Expressions;
using N9.Data.Entities;

namespace N9.Data.Repositories;

/// <summary>
///     Defines the base contract for a repository that manages entities of type <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
public interface IRepositoryBase<T> where T : class, IEntity
{
    /// <summary>
    ///     Adds a new entity to the database and saves changes.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the added entity.</returns>
    Task<int> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds multiple entities to the database and saves changes.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of IDs of the added entities.</returns>
    Task<IEnumerable<int>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates an existing entity in the database and saves changes.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The ID of the updated entity.</returns>
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes an entity from the database by its ID and saves changes.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if the entity was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves an entity from the database by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all entities from the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match entities against.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of matching entities.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    ///   Gets queryable for the entity.
    /// </summary>
    /// <param name="property">A navigation property path.</param>
    /// <returns>A queryable of the entity.</returns>
    IQueryable<T> Query(string? property = null);
}