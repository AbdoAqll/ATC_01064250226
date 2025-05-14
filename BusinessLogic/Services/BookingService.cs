using BusinessLogic.IServices;
using Models.IRepositories;
using Models.Models;
using Models.ViewModels;
using System.Linq.Expressions;
using Utilities;

namespace BusinessLogic.Services
{
    /// <summary>
    /// Service responsible for managing bookings for events, including creating, canceling, and retrieving bookings.
    /// </summary>
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventService _eventService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance to manage transactions.</param>
        /// <param name="eventService">The event service instance to interact with event data.</param>
        public BookingService(IUnitOfWork unitOfWork, IEventService eventService)
        {
            _unitOfWork = unitOfWork;
            _eventService = eventService;
        }

        /// <summary>
        /// Creates a new booking for an event.
        /// </summary>
        /// <param name="eventId">The ID of the event to book.</param>
        /// <param name="userId">The ID of the user making the booking.</param>
        /// <param name="quantity">The number of tickets to book.</param>
        /// <returns>A <see cref="BookingViewModel"/> representing the created booking.</returns>
        /// <exception cref="Exception">Thrown when the event is not found or is in the past.</exception>
        public async Task<BookingViewModel> CreateBooking(int eventId, string userId, int quantity)
        {
            var eventEntity = await _eventService.GetFirstEvent(e => e.Id == eventId, "EventTranslations");
            if (eventEntity == null)
                throw new Exception("Event not found");

            if (eventEntity.Date < DateTime.Today)
                throw new Exception("Cannot book past events");

            var booking = new Booking
            {
                EventId = eventId,
                UserId = userId,
                Quantity = quantity,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.BookingRepository.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            var translation = eventEntity.EventTranslations.FirstOrDefault(t => t.LanguageCode == currentCulture)
                ?? eventEntity.EventTranslations.First();

            return new BookingViewModel
            {
                Id = booking.Id,
                EventId = eventEntity.Id,
                EventName = translation.Name,
                EventCategory = translation.Category,
                EventImageUrl = eventEntity.ImageUrl,
                EventDate = eventEntity.Date,
                EventVenue = translation.Venue,
                EventPrice = eventEntity.Price,
                Quantity = quantity,
                BookingDate = booking.CreatedAt,
                IsCancelled = false
            };
        }

        /// <summary>
        /// Cancels a booking by its ID.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to cancel.</param>
        /// <param name="userId">The ID of the user who owns the booking.</param>
        /// <returns>A boolean indicating whether the cancellation was successful.</returns>
        public async Task<bool> CancelBooking(int bookingId, string userId)
        {
            var booking = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(
                b => b.Id == bookingId && b.UserId == userId);

            if (booking == null)
                return false;

            var eventEntity = await _eventService.GetFirstEvent(e => e.Id == booking.EventId);
            if (eventEntity == null || eventEntity.Date < DateTime.Today)
                return false;

            await _unitOfWork.BookingRepository.HardRemoveAsync(booking);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all bookings for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve bookings for.</param>
        /// <param name="includeProperties">A comma-separated string of related entities to include in the result.</param>
        /// <returns>A collection of <see cref="BookingViewModel"/> for the user's bookings.</returns>
        public async Task<IEnumerable<BookingViewModel>> GetUserBookings(string userId, string? includeProperties = null)
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(
                b => b.UserId == userId && !b.IsDeleted,
                includeProperties ?? "Event,Event.EventTranslations");

            var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            return bookings.Select(b => new BookingViewModel
            {
                Id = b.Id,
                EventId = b.Event.Id,
                EventName = b.Event.EventTranslations
                    .FirstOrDefault(t => t.LanguageCode == currentCulture)?.Name
                    ?? b.Event.EventTranslations.First().Name,
                EventCategory = b.Event.EventTranslations
                    .FirstOrDefault(t => t.LanguageCode == currentCulture)?.Category
                    ?? b.Event.EventTranslations.First().Category,
                EventImageUrl = b.Event.ImageUrl,
                EventDate = b.Event.Date,
                EventVenue = b.Event.EventTranslations
                    .FirstOrDefault(t => t.LanguageCode == currentCulture)?.Venue
                    ?? b.Event.EventTranslations.First().Venue,
                EventPrice = b.Event.Price,
                Quantity = b.Quantity,
                Status = b.Status,
                BookingDate = b.CreatedAt,
                IsCancelled = false,
                CancelledAt = null
            });
        }

        /// <summary>
        /// Retrieves a booking by its ID for a specific user.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to retrieve.</param>
        /// <param name="userId">The ID of the user who owns the booking.</param>
        /// <returns>A <see cref="BookingViewModel"/> representing the booking if found; otherwise, null.</returns>
        public async Task<BookingViewModel?> GetBookingById(int bookingId, string userId)
        {
            var booking = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(
                b => b.Id == bookingId && b.UserId == userId && !b.IsDeleted,
                "Event,Event.EventTranslations");

            if (booking == null)
                return null;

            var currentCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;

            return new BookingViewModel
            {
                Id = booking.Id,
                EventId = booking.Event.Id,
                EventName = booking.Event.EventTranslations
                    .FirstOrDefault(t => t.LanguageCode == currentCulture)?.Name
                    ?? booking.Event.EventTranslations.First().Name,
                EventCategory = booking.Event.EventTranslations
                    .FirstOrDefault(t => t.LanguageCode == currentCulture)?.Category
                    ?? booking.Event.EventTranslations.First().Category,
                EventImageUrl = booking.Event.ImageUrl,
                EventDate = booking.Event.Date,
                EventVenue = booking.Event.EventTranslations
                    .FirstOrDefault(t => t.LanguageCode == currentCulture)?.Venue
                    ?? booking.Event.EventTranslations.First().Venue,
                EventPrice = booking.Event.Price,
                Quantity = booking.Quantity,
                Status = booking.Status,
                BookingDate = booking.CreatedAt,
                IsCancelled = false,
                CancelledAt = null
            };
        }

        /// <summary>
        /// Checks if an event is available for booking (not in the past).
        /// </summary>
        /// <param name="eventId">The ID of the event to check availability for.</param>
        /// <returns>A boolean indicating whether the event is available for booking.</returns>
        public async Task<bool> IsEventAvailable(int eventId)
        {
            var eventEntity = await _eventService.GetFirstEvent(e => e.Id == eventId);
            return eventEntity != null && eventEntity.Date >= DateTime.Today;
        }

        /// <summary>
        /// Retrieves an <see cref="IQueryable"/> collection of bookings for a user, which can be further customized with predicates and includes.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve bookings for.</param>
        /// <param name="includeProperties">A comma-separated string of related entities to include in the result.</param>
        /// <returns>An <see cref="IQueryable"/> of bookings for the user.</returns>
        public IQueryable<Booking> GetUserBookingsQuery(string userId, string? includeProperties = null)
        {
            return _unitOfWork.BookingRepository.GetAllQuery(
                b => b.UserId == userId && !b.IsDeleted,
                includeProperties);
        }

        /// <summary>
        /// Retrieves a list of bookings filtered by the specified criteria.
        /// </summary>
        /// <param name="status">The status of the booking to filter by (e.g., "Confirmed", "Cancelled").</param>
        /// <param name="username">The username of the user to filter bookings by. If null, no filter is applied.</param>
        /// <param name="bookingDate">The date of the booking to filter by. If null, no filter is applied.</param>
        /// <param name="pageNumber">The page number for pagination.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an IEnumerable of <see cref="BookingViewModel"/> objects.</returns>
        public async Task<IEnumerable<BookingViewModel>> GetBookingsWithFiltersAsync(string status, string username, DateTime? bookingDate, int pageNumber)
        {
            Expression<Func<Booking, bool>> predicate = b => true;

            if (!string.IsNullOrEmpty(status))
            {
                predicate = predicate.And(b => b.Status == status);
            }

            if (!string.IsNullOrEmpty(username))
            {
                predicate = predicate.And(b => b.User.UserName.Contains(username));
            }

            if (bookingDate.HasValue)
            {
                predicate = predicate.And(b => b.CreatedAt.Date == bookingDate.Value.Date);
            }

            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(
                predicate: predicate,
                IncludeWord: "User,Event,Event.EventTranslations",
                pageNumber: pageNumber,
                pageSize: 10
            );

            return bookings.Select(b => new BookingViewModel
            {
                Id = b.Id,
                UserId = b.UserId,
                UserName = b.User.UserName,
                EventId = b.EventId,
                EventName = b.Event.EventTranslations.FirstOrDefault(t => t.LanguageCode == "en")?.Name,
                EventImageUrl = b.Event.ImageUrl,
                EventCategory = b.Event.EventTranslations.FirstOrDefault(t => t.LanguageCode == "en")?.Category,
                EventVenue = b.Event.EventTranslations.FirstOrDefault(t => t.LanguageCode == "en")?.Venue,
                EventDate = b.Event.Date,
                EventPrice = b.Event.Price,
                Status = b.Status,
                Quantity = b.Quantity,
                TotalPrice = b.Quantity * b.Event.Price,
                BookingDate = b.CreatedAt,
                IsCancelled = b.Status == StaticStatus.Cancelled,
                CancelledAt = b.Status == StaticStatus.Cancelled ? b.UpdatedAt : null
            });
        }

        /// <summary>
        /// Updates the status of a booking.
        /// </summary>
        /// <param name="id">The ID of the booking to update.</param>
        /// <param name="status">The new status to assign to the booking (e.g., "Confirmed", "Cancelled").</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateBookingStatusAsync(int id, string status)
        {
            var booking = await _unitOfWork.BookingRepository.GetFirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
            {
                return false;
            }

            booking.Status = status;
            booking.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.BookingRepository.UpdateAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

    /// <summary>
    /// Provides extension methods for combining <see cref="Expression{TDelegate}"/> predicates.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Combines two <see cref="Expression{TDelegate}"/> predicates using a logical AND.
        /// </summary>
        /// <typeparam name="T">The type of the entity being queried.</typeparam>
        /// <param name="expr1">The first predicate expression.</param>
        /// <param name="expr2">The second predicate expression to combine with the first.</param>
        /// <returns>A new <see cref="Expression{TDelegate}"/> that represents the combined predicates.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));
            var leftVisitor = new ReplaceParameterVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceParameterVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        /// <summary>
        /// A helper class that visits and replaces the parameters in expressions.
        /// </summary>
        private class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _old;
            private readonly ParameterExpression _new;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReplaceParameterVisitor"/> class.
            /// </summary>
            /// <param name="old">The old parameter expression to replace.</param>
            /// <param name="new">The new parameter expression to replace the old one.</param>
            public ReplaceParameterVisitor(ParameterExpression old, ParameterExpression @new)
            {
                _old = old;
                _new = @new;
            }

            /// <summary>
            /// Visits the <see cref="ParameterExpression"/> and replaces it with the new parameter.
            /// </summary>
            /// <param name="node">The <see cref="ParameterExpression"/> to visit and replace.</param>
            /// <returns>The new parameter expression.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return ReferenceEquals(node, _old) ? _new : base.VisitParameter(node);
            }
        }
    }
}
