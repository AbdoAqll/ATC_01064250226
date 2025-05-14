using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    /// <summary>
    /// Represents an application user with extended properties beyond the default IdentityUser.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// This field is required.
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// This field is required and must follow a valid email format.
        /// </summary>
        [Required]
        [StringLength(256)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public override string Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// This field is required and can only contain letters, numbers, and underscores.
        /// </summary>
        [Required]
        [StringLength(256)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        public override string UserName { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// This field is required and must follow a valid phone number format.
        /// </summary>
        [Required]
        [Phone]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format")]
        public override string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings associated with the user.
        /// </summary>
        public ICollection<Booking> Bookings { get; set; }
    }
}
