using BusinessLogic.IServices;
using Models.IRepositories;

namespace BusinessLogic.Services
{
    /// <summary>
    /// The implementation of <see cref="IServicesProvider"/> that provides access to business logic services such as event and booking management.
    /// </summary>
    public class ServicesProvider : IServicesProvider
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Gets the service for managing events.
        /// </summary>
        public IEventService EventService { get; private set; }

        /// <summary>
        /// Gets the service for managing bookings.
        /// </summary>
        public IBookingService BookingService { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicesProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance to manage transactions.</param>
        /// <param name="eventService">The event service for managing events.</param>
        /// <param name="bookingService">The booking service for managing bookings.</param>
        public ServicesProvider
            (
                IUnitOfWork unitOfWork,
                IEventService eventService,
                IBookingService bookingService
            )
        {
            this.unitOfWork = unitOfWork;
            EventService = eventService;
            BookingService = bookingService;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ServicesProvider"/> instance.
        /// </summary>
        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        /// <summary>
        /// Asynchronously releases all resources used by the <see cref="ServicesProvider"/> instance.
        /// </summary>
        /// <returns>A task representing the asynchronous disposal operation.</returns>
        public ValueTask DisposeAsync()
        {
            return unitOfWork.DisposeAsync();
        }

        /// <summary>
        /// Asynchronously saves changes made in the unit of work.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with the number of affected rows.</returns>
        public Task<int> SaveChangesAsync()
        {
            return unitOfWork.SaveChangesAsync();
        }
    }
}
