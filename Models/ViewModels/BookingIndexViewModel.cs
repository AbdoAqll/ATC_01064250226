namespace Models.ViewModels
{
    /// <summary>
    /// Represents the view model for displaying a user's bookings along with the nearest events.
    /// </summary>
    public class BookingIndexViewModel
    {
        /// <summary>
        /// Gets or sets the paginated list of bookings made by the user.
        /// </summary>
        public PaginatedList<BookingViewModel> UserBookings { get; set; }

        /// <summary>
        /// Gets or sets the list of upcoming events that are nearest to the current date.
        /// </summary>
        public List<EventViewModel> NearestEvents { get; set; }
    }
}
