using System.ComponentModel.DataAnnotations.Schema;
using Utilities;

namespace Models.Models
{
    /// <summary>
    /// Represents a booking made by a user for an event. Inherits from <see cref="BaseModel"/>.
    /// </summary>
    public class Booking : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the booking.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID associated with the booking.
        /// </summary>
        [ForeignKey("User")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user who made the booking.
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Gets or sets the event ID associated with the booking.
        /// </summary>
        [ForeignKey("Event")]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the event for which the booking was made.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Gets or sets the quantity of tickets booked. Default is 1.
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the status of the booking. Possible values include "Pending", "Confirmed", and "Cancelled". Default is "Pending".
        /// </summary>
        public string Status { get; set; } = StaticStatus.Pending;
    }
}
