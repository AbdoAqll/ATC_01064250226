namespace Models.ViewModels
{
    /// <summary>
    /// Represents the view model for the home page, containing paginated upcoming events and soonest events.
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Gets or sets the paginated list of upcoming events.
        /// </summary>
        public PaginatedList<EventViewModel> UpcomingEvents { get; set; }

        /// <summary>
        /// Gets or sets the list of soonest events.
        /// </summary>
        public List<EventViewModel> SoonestEvents { get; set; }
    }
}
