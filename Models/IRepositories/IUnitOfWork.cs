
namespace Models.IRepositories
{
    /// <summary>
    /// Represents the Unit of Work pattern interface to manage multiple repository operations under a single transaction scope.
    /// Ensures that all repository changes are committed together or rolled back on failure.
    /// </summary>
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Asynchronously saves all changes made in the current unit of work to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Gets the repository for managing event-related data operations.
        /// </summary>
        IEventRepository EventRepository { get; }

        /// <summary>
        /// Gets the repository for managing booking-related data operations.
        /// </summary>
        IBookingRepository BookingRepository { get; }

        // Add other repositories here
    }
}
