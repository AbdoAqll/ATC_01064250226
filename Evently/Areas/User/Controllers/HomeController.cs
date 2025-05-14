using BusinessLogic.IServices;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Models.ViewModels;
using System.Diagnostics;

namespace Evently.Areas.User.Controllers
{
    /// <summary>
    /// The HomeController is responsible for managing the home page of the application,
    /// including displaying a list of upcoming events, details of a specific event,
    /// and handling the user's language preference.
    /// </summary>
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServicesProvider _servicesProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging events.</param>
        /// <param name="servicesProvider">The service provider to access event-related services.</param>
        public HomeController(ILogger<HomeController> logger, IServicesProvider servicesProvider)
        {
            _logger = logger;
            _servicesProvider = servicesProvider;
        }

        /// <summary>
        /// Retrieves and displays upcoming events in a paginated format.
        /// Additionally, it shows the soonest upcoming events.
        /// </summary>
        /// <param name="pageNumber">The page number for pagination, defaults to 1 if not provided.</param>
        /// <returns>An IActionResult that returns a view with the events.</returns>
        public async Task<IActionResult> Index(int? pageNumber)
        {
            // Get the current culture to determine which translations to use
            var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            // Retrieve all upcoming events
            var upcomingEventsQuery = _servicesProvider.EventService.GetAllEventsQuery(
                predicate: e => e.Date >= DateTime.Today,
                includeProperties: "EventTranslations,Bookings"
            );

            // Calculate total count for pagination
            var totalCount = await upcomingEventsQuery.CountAsync();
            var pageSize = 6; // Limit to 6 events per page
            var currentPage = pageNumber ?? 1;

            // Retrieve the events for the current page
            var upcomingEvents = await upcomingEventsQuery
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get current user's bookings
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userBookedEventIds = new HashSet<int>();

            if (!string.IsNullOrEmpty(currentUserId))
            {
                var bookedEventIds = await _servicesProvider.BookingService
                    .GetUserBookingsQuery(currentUserId)
                    .Select(b => b.EventId)
                    .ToListAsync();
                userBookedEventIds = new HashSet<int>(bookedEventIds);
            }

            // Map events to view models
            var upcomingEventViewModels = upcomingEvents
                .Select(e => {
                    var viewModel = EventViewModelMapper.MapToEventViewModel(e, currentCulture);
                    viewModel.IsBooked = userBookedEventIds.Contains(e.Id);
                    return viewModel;
                })
                .ToList();

            // Create a paginated list of events
            var paginatedUpcomingEvents = new PaginatedList<EventViewModel>(
                upcomingEventViewModels,
                totalCount,
                currentPage,
                pageSize
            );

            // Retrieve the next 2 soonest upcoming events
            var soonestEvents = await _servicesProvider.EventService
                .GetAllEventsQuery(
                    predicate: e => e.Date >= DateTime.Today,
                    includeProperties: "EventTranslations,Bookings"
                )
                .OrderBy(e => e.Date)
                .Take(2)
                .ToListAsync();

            // Map soonest events to view models
            var soonestEventViewModels = soonestEvents
                .Select(e => {
                    var viewModel = EventViewModelMapper.MapToEventViewModel(e, currentCulture);
                    viewModel.IsBooked = userBookedEventIds.Contains(e.Id);
                    return viewModel;
                })
                .ToList();

            // Prepare the view model with both upcoming and soonest events
            var viewModel = new HomeViewModel
            {
                UpcomingEvents = paginatedUpcomingEvents,
                SoonestEvents = soonestEventViewModels
            };

            return View(viewModel);
        }

        /// <summary>
        /// Displays the details of a specific event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to display details for.</param>
        /// <returns>An IActionResult that returns the event details view.</returns>
        public async Task<IActionResult> Details(int id)
        {
            // Retrieve the event by its ID, including translations
            var eventEntity = await _servicesProvider.EventService.GetFirstEvent(e => e.Id == id, "EventTranslations");
            if (eventEntity == null)
            {
                TempData["Error"] = "Event not found.";
                return RedirectToAction(nameof(Index));
            }

            // Get current culture and select appropriate translation for the event
            var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            var translation = eventEntity.EventTranslations.FirstOrDefault(t => t.LanguageCode == currentCulture)
                ?? eventEntity.EventTranslations.First();

            // Check if the current user has booked this event
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isBooked = false;

            if (!string.IsNullOrEmpty(currentUserId))
            {
                isBooked = await _servicesProvider.BookingService
                    .GetUserBookingsQuery(currentUserId)
                    .AnyAsync(b => b.EventId == id && !b.IsDeleted);
            }

            // Map event to the view model and set booking status
            var viewModel = new EventViewModel
            {
                Id = eventEntity.Id,
                Name = translation.Name,
                Category = translation.Category,
                Description = translation.Description,
                Venue = translation.Venue,
                Date = eventEntity.Date,
                Price = eventEntity.Price,
                ImageUrl = eventEntity.ImageUrl,
                GoogleMapUrl = eventEntity.GoogleMapUrl,
                Tags = translation.EventTags,
                IsBooked = isBooked
            };

            return View(viewModel);
        }

        /// <summary>
        /// Sets the language for the user interface based on the provided culture code.
        /// </summary>
        /// <param name="culture">The culture code (e.g., "en", "fr").</param>
        /// <returns>A redirect to the referring page after setting the language preference.</returns>
        public IActionResult SetLanguage(string culture)
        {
            // Store the selected language in a cookie for one year
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Redirect the user to the previous page
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }

    /// <summary>
    /// Static mapper class for converting event entities to event view models.
    /// </summary>
    public static class EventViewModelMapper
    {
        /// <summary>
        /// Maps an event entity to an event view model based on the current culture.
        /// </summary>
        /// <param name="eventEntity">The event entity to map.</param>
        /// <param name="cultureCode">The culture code used to select the correct event translation.</param>
        /// <returns>An event view model.</returns>
        public static EventViewModel MapToEventViewModel(Event eventEntity, string cultureCode)
        {
            var translation = eventEntity.EventTranslations.FirstOrDefault(t => t.LanguageCode == cultureCode)
                ?? eventEntity.EventTranslations.First(); // Fallback to the first translation if the current culture is not found

            return new EventViewModel
            {
                Id = eventEntity.Id,
                Name = translation.Name,
                Category = translation.Category,
                Description = translation.Description,
                Venue = translation.Venue,
                Tags = translation.EventTags,
                ImageUrl = eventEntity.ImageUrl,
                GoogleMapUrl = eventEntity.GoogleMapUrl,
                Date = eventEntity.Date,
                Price = eventEntity.Price
            };
        }
    }
}
