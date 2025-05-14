using BusinessLogic.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace Evently.Areas.Admin.Controllers
{
    /// <summary>
    /// The controller responsible for managing bookings in the admin area.
    /// It includes methods for displaying a list of bookings with filters and updating booking status.
    /// The services used by the controller are resolved through the provided services provider.
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingController : Controller
    {
        private readonly IServicesProvider _servicesProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// This constructor takes an <see cref="IServicesProvider"/> that is used to resolve services dynamically.
        /// </summary>
        /// <param name="servicesProvider">The service provider used to resolve the necessary services for booking management.</param>
        public BookingController(IServicesProvider servicesProvider)
        {
            _servicesProvider = servicesProvider;
        }

        /// <summary>
        /// Displays a list of bookings with optional filters for status, username, booking date, and pagination.
        /// The bookings are retrieved using the <see cref="IBookingService"/> resolved through the <see cref="IServicesProvider"/>.
        /// </summary>
        /// <param name="status">The status filter for bookings (optional).</param>
        /// <param name="username">The username filter for bookings (optional).</param>
        /// <param name="bookingDate">The booking date filter (optional).</param>
        /// <param name="pageNumber">The page number for pagination (optional, default is 1).</param>
        /// <returns>An <see cref="IActionResult"/> representing the view with a list of filtered bookings.</returns>
        public async Task<IActionResult> Index(string status = null, string username = null, DateTime? bookingDate = null, int pageNumber = 1)
        {
            var bookingService = _servicesProvider.BookingService; // Resolving the IBookingService dynamically
            var bookingViewModels = await bookingService.GetBookingsWithFiltersAsync(status, username, bookingDate, pageNumber);

            ViewBag.StatusFilter = status;
            ViewBag.UsernameFilter = username;
            ViewBag.BookingDateFilter = bookingDate;
            ViewBag.Statuses = new List<string> { StaticStatus.Pending, StaticStatus.Confirmed, StaticStatus.Cancelled };

            return View(bookingViewModels);
        }

        /// <summary>
        /// Updates the status of a booking.
        /// The booking status is updated using the <see cref="IBookingService"/> resolved through the <see cref="IServicesProvider"/>.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="status">The new status to apply to the booking.</param>
        /// <returns>An <see cref="IActionResult"/> that redirects to the index page if the update is successful, or returns a 404 if the booking is not found.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var bookingService = _servicesProvider.BookingService; // Resolving the IBookingService dynamically
            var result = await bookingService.UpdateBookingStatusAsync(id, status);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
