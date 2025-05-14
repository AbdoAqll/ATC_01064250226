using Models.Models;

namespace Models.IRepositories
{
    /// <summary>
    /// Represents a repository interface for managing <see cref="Booking"/> entities.
    /// Inherits basic CRUD and query operations from <see cref="IGenericRepository{Booking}"/>.
    /// Add booking-specific data access methods here if needed.
    /// </summary>
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        // Add custom methods specific to Booking here, if necessary
    }
}
