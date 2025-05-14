using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Models
{
    /// <summary>
    /// Represents the translations of an event's details in different languages.
    /// </summary>
    public class EventTranslations
    {
        /// <summary>
        /// Gets or sets the unique identifier for the event translation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the event ID associated with this translation.
        /// </summary>
        [ForeignKey("Event")]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the event that this translation belongs to.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// Gets or sets the language code for the translation (e.g., "en" for English, "fr" for French).
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the event in the specific language.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the event in the specific language.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category of the event in the specific language.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the venue of the event in the specific language.
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// Gets or sets the event tags associated with the event in the specific language.
        /// </summary>
        public string EventTags { get; set; }
    }
}
