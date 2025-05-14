
namespace BusinessLogic.IServices
{
    /// <summary>
    /// Provides access to the different services used in the application, 
    /// such as <see cref="IEventService"/> and <see cref="IBookingService"/>.
    /// </summary>
    public interface IServicesProvider : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Gets the service for managing events.
        /// </summary>
        IEventService EventService { get; }

        /// <summary>
        /// Gets the service for managing bookings.
        /// </summary>
        IBookingService BookingService { get; }

        /// <summary>
        /// Saves all changes made in the context asynchronously.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();
    }
}
