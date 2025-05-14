namespace Models.Models
{
    /// <summary>
    /// Represents an event in the system. Inherits from <see cref="BaseModel"/>.
    /// </summary>
    public class Event : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time of the event.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the Google Maps URL for the event location.
        /// </summary>
        public string GoogleMapUrl { get; set; }

        /// <summary>
        /// Gets or sets the price for attending the event.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the image URL representing the event.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings for this event.
        /// </summary>
        public ICollection<Booking>? Bookings { get; set; }

        /// <summary>
        /// Gets or sets the collection of translations for the event, to handle multiple languages.
        /// </summary>
        public ICollection<EventTranslations> EventTranslations { get; set; }
    }
}
