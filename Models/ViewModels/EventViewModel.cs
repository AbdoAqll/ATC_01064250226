namespace Models.ViewModels
{
    /// <summary>
    /// Represents the view model for an event with details like name, category, description, and more.
    /// </summary>
    public class EventViewModel
    {
        /// <summary>
        /// Gets or sets the event's unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category of the event.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the venue of the event.
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Gets or sets the tags associated with the event.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the URL of the event's image.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the Google Maps URL for the event location.
        /// </summary>
        public string GoogleMapUrl { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the event.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the price of the event.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is booked.
        /// </summary>
        public bool IsBooked { get; set; }
    }
}
