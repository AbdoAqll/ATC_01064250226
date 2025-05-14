using BusinessLogic.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;
using System.Security.Claims;
using Utilities;

namespace Evently.Areas.User.Controllers
{
    /// <summary>
    /// Controller for managing event bookings.
    /// </summary>
    [Area("User")]
    [Authorize(Roles = StaticRoles.UserRole)]
    public class BookingController : Controller
    {
        private readonly IServicesProvider _servicesProvider;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHtmlLocalizer<BookingController> _localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="servicesProvider">The services provider for accessing business logic services.</param>
        /// <param name="userManager">The user manager for managing user authentication.</param>
        /// <param name="logger">The logger for logging activities and errors.</param>
        /// <param name="localizer">The localizer for handling localized content.</param>
        public BookingController(
            IServicesProvider servicesProvider, 
            UserManager<ApplicationUser> userManager, 
            IHtmlLocalizer<BookingController> localizer)
        {
            _servicesProvider = servicesProvider;
            _userManager = userManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Displays the list of user's bookings with pagination.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <returns>A view containing the user's bookings and nearest events.</returns>
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            // Get user's bookings with event details
            var userBookingsQuery = _servicesProvider.BookingService.GetUserBookingsQuery(userId);

            // Get total count for pagination
            var totalCount = await userBookingsQuery.CountAsync();
            var pageSize = 6; // Limit to 6 bookings per page
            var currentPage = pageNumber ?? 1;

            // Get paginated bookings
            var bookings = await userBookingsQuery
                .OrderByDescending(b => b.CreatedAt)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookingViewModel
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    EventId = b.EventId,
                    EventName = b.Event.EventTranslations
                        .Where(t => t.LanguageCode == currentCulture)
                        .Select(t => t.Name)
                        .FirstOrDefault() ?? b.Event.EventTranslations.First().Name,
                    EventImageUrl = b.Event.ImageUrl,
                    EventCategory = b.Event.EventTranslations
                        .Where(t => t.LanguageCode == currentCulture)
                        .Select(t => t.Category)
                        .FirstOrDefault() ?? b.Event.EventTranslations.First().Category,
                    EventVenue = b.Event.EventTranslations
                        .Where(t => t.LanguageCode == currentCulture)
                        .Select(t => t.Venue)
                        .FirstOrDefault() ?? b.Event.EventTranslations.First().Venue,
                    EventDate = b.Event.Date,
                    EventPrice = b.Event.Price,
                    Status = b.Status,
                    Quantity = b.Quantity,
                    TotalPrice = b.Event.Price * b.Quantity,
                    BookingDate = b.CreatedAt,
                    IsCancelled = false,
                    CancelledAt = null,
                    Event = new EventViewModel
                    {
                        Id = b.Event.Id,
                        Name = b.Event.EventTranslations
                            .Where(t => t.LanguageCode == currentCulture)
                            .Select(t => t.Name)
                            .FirstOrDefault() ?? b.Event.EventTranslations.First().Name,
                        Category = b.Event.EventTranslations
                            .Where(t => t.LanguageCode == currentCulture)
                            .Select(t => t.Category)
                            .FirstOrDefault() ?? b.Event.EventTranslations.First().Category,
                        Date = b.Event.Date,
                        ImageUrl = b.Event.ImageUrl,
                        Price = b.Event.Price
                    }
                })
                .ToListAsync();

            var userBookings = new PaginatedList<BookingViewModel>(bookings, totalCount, currentPage, pageSize);

            // Get nearest events from user's bookings
            var nearestEvents = await _servicesProvider.BookingService.GetUserBookingsQuery(
                userId: userId,
                includeProperties: "Event,Event.EventTranslations"
            )
            .Where(b => !b.IsDeleted && b.Event.Date >= DateTime.Today)
            .OrderBy(b => b.Event.Date)
            .Take(2)
            .Select(b => new EventViewModel
            {
                Id = b.Event.Id,
                Name = b.Event.EventTranslations
                    .Where(t => t.LanguageCode == currentCulture)
                    .Select(t => t.Name)
                    .FirstOrDefault() ?? b.Event.EventTranslations.First().Name,
                Category = b.Event.EventTranslations
                    .Where(t => t.LanguageCode == currentCulture)
                    .Select(t => t.Category)
                    .FirstOrDefault() ?? b.Event.EventTranslations.First().Category,
                Date = b.Event.Date,
                ImageUrl = b.Event.ImageUrl,
                Price = b.Event.Price
            })
            .ToListAsync();

            var viewModel = new BookingIndexViewModel
            {
                UserBookings = userBookings,
                NearestEvents = nearestEvents
            };

            return View(viewModel);
        }

        /// <summary>
        /// Displays the booking page for a specific event.
        /// </summary>
        /// <param name="id">The event ID to book.</param>
        /// <returns>A view to confirm booking of the event.</returns>
        [HttpGet]
        public async Task<IActionResult> Book(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            // Check if event is available
            if (!await _servicesProvider.BookingService.IsEventAvailable(id))
            {
                TempData["Error"] = _localizer["EventNotAvailable"].Value;
                return RedirectToAction("Index", "Home");
            }

            // Get event details for confirmation
            var eventEntity = await _servicesProvider.EventService.GetFirstEvent(e => e.Id == id, "EventTranslations");
            if (eventEntity == null)
            {
                TempData["Error"] = _localizer["EventNotFound"].Value;
                return RedirectToAction("Index", "Home");
            }

            var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            var translation = eventEntity.EventTranslations.FirstOrDefault(t => t.LanguageCode == currentCulture)
                ?? eventEntity.EventTranslations.First();

            var viewModel = new BookingViewModel
            {
                EventId = eventEntity.Id,
                EventName = translation.Name,
                EventCategory = translation.Category,
                EventVenue = translation.Venue,
                EventImageUrl = eventEntity.ImageUrl,
                EventDate = eventEntity.Date,
                EventPrice = eventEntity.Price,
                Quantity = 1 // Default quantity
            };

            return View(viewModel);
        }

        /// <summary>
        /// Processes the booking request for a specific event.
        /// </summary>
        /// <param name="id">The event ID to book.</param>
        /// <param name="quantity">The number of tickets to book.</param>
        /// <returns>A view showing the booking confirmation or an error.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(int id, [FromForm] int quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            try
            {
                if (quantity <= 0)
                {
                    ModelState.AddModelError("Quantity", _localizer["InvalidQuantity"].Value);
                    return View();
                }

                var booking = await _servicesProvider.BookingService.CreateBooking(id, userId, quantity);
                return RedirectToAction(nameof(Success), new { id = booking.Id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = _localizer["BookingError"].Value;
                return RedirectToAction(nameof(Book), new { id });
            }
        }

        /// <summary>
        /// Displays the booking success page for a specific booking.
        /// </summary>
        /// <param name="id">The booking ID.</param>
        /// <returns>A view showing the booking details.</returns>
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            var booking = await _servicesProvider.BookingService.GetBookingById(id, userId);
            if (booking == null)
            {
                TempData["Error"] = _localizer["BookingNotFound"].Value;
                return RedirectToAction(nameof(Index));
            }

            return View(booking);
        }

        /// <summary>
        /// Cancels an existing booking.
        /// </summary>
        /// <param name="id">The booking ID to cancel.</param>
        /// <returns>A redirect to the booking index page or an error message.</returns>
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var booking = await _servicesProvider.BookingService.GetBookingById(id, userId);

            if (booking == null)
            {
                TempData["Error"] = _localizer["BookingNotFound"].Value;
                return RedirectToAction(nameof(Index));
            }

            if (booking.Status == StaticStatus.Cancelled || booking.EventDate <= DateTime.Today)
            {
                TempData["Error"] = _localizer["CannotCancelBooking"].Value;
                return RedirectToAction(nameof(Index));
            }

            var result = await _servicesProvider.BookingService.CancelBooking(id, userId);
            if (!result)
            {
                TempData["Error"] = _localizer["CannotCancelBooking"].Value;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = _localizer["BookingCancelled"].Value;
            return RedirectToAction(nameof(Index));
        }
    }
}