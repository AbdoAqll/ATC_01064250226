using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    /// <summary>
    /// Represents the view model for creating or updating an event.
    /// </summary>
    public class EventCreateUpdateViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the event.
        /// This is only required for updating an existing event.
        /// </summary>
        public int Id { get; set; } // Only needed for updates

        /// <summary>
        /// Gets or sets the date and time of the event.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the price of the event.
        /// This is a required field and must be a positive number.
        /// </summary>
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the Google Maps URL for the event location.
        /// </summary>
        public string GoogleMapUrl { get; set; }

        /// <summary>
        /// Gets or sets the image URL for the event.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of event translations associated with the event.
        /// This list is never null and contains multiple language translations for the event.
        /// </summary>
        // Initialize the list to ensure it's never null
        public List<EventTranslationViewModel> EventTranslations { get; set; }

        /// <summary>
        /// Represents the translation details for an event in a specific language.
        /// </summary>
        public class EventTranslationViewModel
        {
            /// <summary>
            /// Gets or sets the unique identifier for the event translation.
            /// This is only required for updating an existing translation.
            /// </summary>
            public int Id { get; set; } // Only needed for updates

            /// <summary>
            /// Gets or sets the language code of the translation (e.g., "en" for English, "fr" for French).
            /// </summary>
            [Required]
            public string LanguageCode { get; set; }

            /// <summary>
            /// Gets or sets the translated name of the event.
            /// </summary>
            [Required]
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the translated description of the event.
            /// </summary>
            [Required]
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the translated category of the event.
            /// </summary>
            [Required]
            public string Category { get; set; }

            /// <summary>
            /// Gets or sets the translated venue of the event.
            /// </summary>
            [Required]
            public string Venue { get; set; }

            /// <summary>
            /// Gets or sets the translated event tags.
            /// </summary>
            [Required]
            public string EventTags { get; set; }
        }
    }
}
