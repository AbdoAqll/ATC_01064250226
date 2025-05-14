namespace Models.Models
{
    /// <summary>
    /// Represents the base model for all entities that require auditing and soft delete functionality.
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// This field is set to the current UTC time by default.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Default to now

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// This field is nullable because it may not be updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; } // Nullable because it may not be updated

        /// <summary>
        /// Gets or sets the date and time when the entity was deleted.
        /// This field is nullable because it may not be deleted.
        /// </summary>
        public DateTime? DeletedAt { get; set; } // Nullable because it may not be deleted

        /// <summary>
        /// Gets or sets a value indicating whether the entity is marked as deleted.
        /// This is used for soft deletion.
        /// </summary>
        public bool IsDeleted { get; set; } = false; // Soft delete flag

        /// <summary>
        /// Marks the entity as deleted and sets the deleted date to the current UTC time.
        /// </summary>
        public void MarkAsDeleted()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
