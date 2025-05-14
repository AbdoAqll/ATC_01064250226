using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using Models.IRepositories;
using Models.Models;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Represents a generic repository for performing CRUD operations on entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity that the repository manages.</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly ApplicationDbContext context;
        protected DbSet<T> dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        /// <summary>
        /// Retrieves all entities with optional filtering and eager loading.
        /// </summary>
        /// <param name="predicate">The filtering predicate to apply to the query, or <c>null</c> for no filtering.</param>
        /// <param name="IncludeWord">A comma-separated string specifying related entities to include in the query, or <c>null</c> for no eager loading.</param>
        /// <returns>A queryable collection of entities.</returns>
        public IQueryable<T> GetAllQuery(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = dbSet.Where(e => !e.IsDeleted); // Exclude soft-deleted records

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query;
        }

        /// <summary>
        /// Adds a new entity to the database asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Retrieves a list of all entities asynchronously with optional filtering, eager loading, and pagination.
        /// </summary>
        /// <param name="predicate">The filtering predicate to apply to the query, or <c>null</c> for no filtering.</param>
        /// <param name="IncludeWord">A comma-separated string specifying related entities to include in the query, or <c>null</c> for no eager loading.</param>
        /// <param name="pageNumber">The page number to retrieve, or <c>null</c> for no pagination.</param>
        /// <param name="pageSize">The number of items per page, or <c>null</c> for no pagination.</param>
        /// <returns>A list of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null, int? pageNumber = null, int? pageSize = null)
        {
            IQueryable<T> query = dbSet.Where(e => !e.IsDeleted); // Exclude soft-deleted records

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int skip = (pageNumber.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Retrieves a list of soft-deleted entities asynchronously with optional filtering, eager loading, and pagination.
        /// </summary>
        /// <param name="predicate">The filtering predicate to apply to the query, or <c>null</c> for no filtering.</param>
        /// <param name="IncludeWord">A comma-separated string specifying related entities to include in the query, or <c>null</c> for no eager loading.</param>
        /// <param name="pageNumber">The page number to retrieve, or <c>null</c> for no pagination.</param>
        /// <param name="pageSize">The number of items per page, or <c>null</c> for no pagination.</param>
        /// <returns>A list of soft-deleted entities.</returns>
        public async Task<IEnumerable<T>> GetAllTrashAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null, int? pageNumber = null, int? pageSize = null)
        {
            IQueryable<T> query = dbSet.Where(e => e.IsDeleted == true); // Include soft-deleted records

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                int skip = (pageNumber.Value - 1) * pageSize.Value;
                query = query.Skip(skip).Take(pageSize.Value);
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Retrieves the first entity that matches the specified predicate asynchronously, excluding soft-deleted entities.
        /// </summary>
        /// <param name="predicate">The filtering predicate to apply to the query, or <c>null</c> for no filtering.</param>
        /// <param name="IncludeWord">A comma-separated string specifying related entities to include in the query, or <c>null</c> for no eager loading.</param>
        /// <returns>The first matching entity, or <c>null</c> if no entity is found.</returns>
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, string? IncludeWord = null)
        {
            IQueryable<T> query = dbSet.Where(e => !e.IsDeleted); // Exclude soft-deleted records

            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (IncludeWord != null)
            {
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return (await query.FirstOrDefaultAsync())!;
        }

        /// <summary>
        /// Marks an entity as soft-deleted by setting the <see cref="BaseModel.IsDeleted"/> property to true.
        /// </summary>
        /// <param name="entity">The entity to mark as deleted.</param>
        /// <returns>An asynchronous task representing the operation.</returns>
        public async Task SoftRemoveAsync(T entity)
        {
            entity.MarkAsDeleted(); // Set IsDeleted = true and DeletedAt = now
            await UpdateAsync(entity); // Persist changes
        }

        /// <summary>
        /// Permanently removes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>An asynchronous task representing the operation.</returns>
        public Task HardRemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an entity and saves changes to the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        public async Task<T> UpdateAsync(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
