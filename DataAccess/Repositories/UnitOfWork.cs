using DataAccess.DBContext;
using Models.IRepositories;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Provides a concrete implementation of the <see cref="IUnitOfWork"/> interface.
    /// Manages the lifecycle of the database context and coordinates changes across multiple repositories.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Gets the repository for managing event-related operations.
        /// </summary>
        public IEventRepository EventRepository { get; private set; }

        /// <summary>
        /// Gets the repository for managing booking-related operations.
        /// </summary>
        public IBookingRepository BookingRepository { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified database context and repositories.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="eventRepository">The event repository.</param>
        /// <param name="bookingRepository">The booking repository.</param>
        public UnitOfWork(
            ApplicationDbContext context,
            IEventRepository eventRepository,
            IBookingRepository bookingRepository)
        {
            this.context = context;
            EventRepository = eventRepository;
            BookingRepository = bookingRepository;
        }

        /// <summary>
        /// Disposes the database context.
        /// </summary>
        public void Dispose()
        {
            context.Dispose();
        }

        /// <summary>
        /// Asynchronously disposes the database context.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
        public ValueTask DisposeAsync()
        {
            return context.DisposeAsync();
        }

        /// <summary>
        /// Asynchronously saves all pending changes in the repositories to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
