using System.Linq.Expressions;

namespace Models.IRepositories
{
    /// <summary>
    /// Defines a generic repository interface for performing common data access operations
    /// such as retrieval, insertion, update, and deletion for a given entity type.
    /// </summary>
    /// <typeparam name="T">The entity type for which this repository is responsible.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities matching the specified predicate with optional related data, paging support, and filtering.
        /// </summary>
        /// <param name="predicate">Optional condition to filter entities.</param>
        /// <param name="IncludeWord">Optional navigation property to include related data.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null, int? pageNumber = null, int? pageSize = null);

        /// <summary>
        /// Retrieves all soft-deleted (trashed) entities matching the specified predicate with optional related data and pagination.
        /// </summary>
        /// <param name="predicate">Optional condition to filter entities.</param>
        /// <param name="IncludeWord">Optional navigation property to include related data.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a collection of soft-deleted entities.</returns>
        Task<IEnumerable<T>> GetAllTrashAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null, int? pageNumber = null, int? pageSize = null);

        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> for querying the entities based on the given predicate and including related data.
        /// </summary>
        /// <param name="predicate">Optional condition to filter entities.</param>
        /// <param name="IncludeWord">Optional navigation property to include related data.</param>
        /// <returns>An <see cref="IQueryable{T}"/> to allow further LINQ operations.</returns>
        IQueryable<T> GetAllQuery(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);

        /// <summary>
        /// Asynchronously retrieves the first entity that matches the specified predicate with optional related data.
        /// </summary>
        /// <param name="predicate">Optional condition to filter entities.</param>
        /// <param name="IncludeWord">Optional navigation property to include related data.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the matched entity or null if not found.</returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null);

        /// <summary>
        /// Asynchronously adds a new entity to the data source.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the added entity.</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Marks an entity as deleted without physically removing it from the data source (soft delete).
        /// </summary>
        /// <param name="entity">The entity to be soft-deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SoftRemoveAsync(T entity);

        /// <summary>
        /// Permanently removes an entity from the data source (hard delete).
        /// </summary>
        /// <param name="entity">The entity to be permanently deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task HardRemoveAsync(T entity);

        /// <summary>
        /// Asynchronously updates an existing entity in the data source.
        /// </summary>
        /// <param name="entity">The entity with updated values.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the updated entity.</returns>
        Task<T> UpdateAsync(T entity);
    }
}
