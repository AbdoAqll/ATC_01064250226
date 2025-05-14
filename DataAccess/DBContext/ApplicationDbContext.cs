using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Models.Models;

namespace DataAccess.DBContext
{
    /// <summary>
    /// Represents the application database context, inheriting from IdentityDbContext to handle user identities.
    /// Configures relationships between Events, Bookings, and EventTranslations.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">Options for configuring the <see cref="DbContext"/>.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Events in the database.
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// Gets or sets the Bookings in the database.
        /// </summary>
        public DbSet<Booking> Bookings { get; set; }

        /// <summary>
        /// Gets or sets the EventTranslations in the database.
        /// </summary>
        public DbSet<EventTranslations> EventTranslations { get; set; }

        /// <summary>
        /// Configures the model for the application.
        /// This method is called by the runtime to configure entity types.
        /// </summary>
        /// <param name="builder">The model builder used to configure the entities.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Event → Booking (one-to-many)
            builder.Entity<Event>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Event>()
                .HasMany(e => e.EventTranslations)
                .WithOne(t => t.Event)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Booking (one-to-many)
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}

