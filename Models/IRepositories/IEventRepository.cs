using Models.Models;

namespace Models.IRepositories
{
    /// <summary>
    /// Represents a repository interface for managing <see cref="Event"/> entities.
    /// Inherits basic CRUD and query operations from <see cref="IGenericRepository{Event}"/>.
    /// Add event-specific data access methods here if needed.
    /// </summary>
    public interface IEventRepository : IGenericRepository<Event>
    {
        // Add custom methods specific to Event here, if necessary
    }
}
