namespace Models.ViewModels
{
    /// <summary>
    /// Represents the view model for displaying booking details.
    /// </summary>
    public class BookingViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the booking.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who made the booking.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who made the booking.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the event for which the booking was made.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the image URL for the event.
        /// </summary>
        public string EventImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the category of the event.
        /// </summary>
        public string EventCategory { get; set; }

        /// <summary>
        /// Gets or sets the venue of the event.
        /// </summary>
        public string EventVenue { get; set; }

        /// <summary>
        /// Gets or sets the date of the event.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets the price of the event.
        /// </summary>
        public decimal EventPrice { get; set; }

        /// <summary>
        /// Gets or sets the status of the booking (e.g., "Pending", "Confirmed", "Cancelled").
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the quantity of tickets booked.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the total price for the booking (Price * Quantity).
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the event associated with the booking.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the date when the booking was made.
        /// </summary>
        public DateTime BookingDate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the booking has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the booking was cancelled, if applicable.
        /// </summary>
        public DateTime? CancelledAt { get; set; }

        /// <summary>
        /// Gets or sets the detailed event information for the booking.
        /// </summary>
        public EventViewModel Event { get; set; }
    }
}
