using Microsoft.EntityFrameworkCore;

namespace Models.ViewModels
{
    /// <summary>
    /// Represents a paginated list of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the list.</typeparam>
    public class PaginatedList<T>
    {
        /// <summary>
        /// Gets the items in the current page.
        /// </summary>
        public List<T> Items { get; }

        /// <summary>
        /// Gets the current page index (1-based).
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Gets the total number of items in the source collection.
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Gets a value indicating whether there is a previous page.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// Gets a value indicating whether there is a next page.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginatedList{T}"/> class.
        /// </summary>
        /// <param name="items">The items to be displayed in the current page.</param>
        /// <param name="count">The total count of items in the source collection.</param>
        /// <param name="pageIndex">The current page index (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalItems = count;
            Items = items;
        }

        /// <summary>
        /// Creates a paginated list asynchronously from an <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <param name="source">The source queryable collection.</param>
        /// <param name="pageIndex">The current page index (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedList{T}"/> containing the items for the current page.</returns>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
