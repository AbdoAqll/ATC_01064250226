using Models.Models;
using Models.ViewModels;

namespace BusinessLogic.IServices
{
    /// <summary>
    /// Defines the service layer methods for managing <see cref="Booking"/> entities.
    /// </summary>
    public interface IBookingService
    {
        /// <summary>
        /// Creates a new booking for a specific event and user.
        /// </summary>
        /// <param name="eventId">The identifier of the event to book.</param>
        /// <param name="userId">The identifier of the user making the booking.</param>
        /// <param name="quantity">The quantity of tickets to book.</param>
        /// <returns>A task representing the asynchronous operation, with the created booking's view model.</returns>
        Task<BookingViewModel> CreateBooking(int eventId, string userId, int quantity);

        /// <summary>
        /// Cancels a booking by its identifier and user.
        /// </summary>
        /// <param name="bookingId">The identifier of the booking to cancel.</param>
        /// <param name="userId">The identifier of the user who made the booking.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
        Task<bool> CancelBooking(int bookingId, string userId);

        /// <summary>
        /// Retrieves all bookings for a user.
        /// </summary>
        /// <param name="userId">The identifier of the user to retrieve bookings for.</param>
        /// <param name="includeProperties">Comma-separated list of related properties to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of booking view models for the user.</returns>
        Task<IEnumerable<BookingViewModel>> GetUserBookings(string userId, string? includeProperties = null);

        /// <summary>
        /// Retrieves a booking by its identifier for a specific user.
        /// </summary>
        /// <param name="bookingId">The identifier of the booking to retrieve.</param>
        /// <param name="userId">The identifier of the user who made the booking.</param>
        /// <returns>A task representing the asynchronous operation, with the booking view model if found, otherwise null.</returns>
        Task<BookingViewModel?> GetBookingById(int bookingId, string userId);

        /// <summary>
        /// Checks if there are available seats for a specific event.
        /// </summary>
        /// <param name="eventId">The identifier of the event to check for availability.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating if the event has availability.</returns>
        Task<bool> IsEventAvailable(int eventId);

        /// <summary>
        /// Retrieves all bookings for a user as an <see cref="IQueryable"/>, with optional related properties.
        /// </summary>
        /// <param name="userId">The identifier of the user to retrieve bookings for.</param>
        /// <param name="includeProperties">Comma-separated list of related properties to include in the query.</param>
        /// <returns>An <see cref="IQueryable"/> of bookings for the user.</returns>
        IQueryable<Booking> GetUserBookingsQuery(string userId, string? includeProperties = null);

        /// <summary>
        /// Retrieves a list of bookings filtered by the specified criteria.
        /// </summary>
        /// <param name="status">The status of the booking to filter by (e.g., "Confirmed", "Cancelled").</param>
        /// <param name="username">The username of the user to filter bookings by. If null, no filter is applied.</param>
        /// <param name="bookingDate">The date of the booking to filter by. If null, no filter is applied.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an IEnumerable of <see cref="BookingViewModel"/> objects.</returns>
        Task<IEnumerable<BookingViewModel>> GetBookingsWithFiltersAsync(string status, string username, DateTime? bookingDate, int pageNumber);

        /// <summary>
        /// Updates the status of a booking.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="status">The new status to assign to the booking (e.g., "Confirmed", "Cancelled").</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateBookingStatusAsync(int id, string status);
    }
}
