using Models.Models;
using Models.ViewModels;
using System.Linq.Expressions;

namespace BusinessLogic.IServices
{
    /// <summary>
    /// Defines the service layer methods for managing <see cref="Event"/> entities.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Retrieves all events asynchronously, with optional filtering, including related properties, and pagination.
        /// </summary>
        /// <param name="predicate">Optional filter for events.</param>
        /// <param name="includeProperties">Comma-separated list of related properties to include.</param>
        /// <param name="pageNumber">Optional page number for pagination.</param>
        /// <param name="pageSize">Optional page size for pagination.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of events.</returns>
        public Task<IEnumerable<Event>> GetAllEvents(Expression<Func<Event, bool>>? predicate = null, string? includeProperties = null, int? pageNumber = null, int? pageSize = null);

        /// <summary>
        /// Retrieves all events as an <see cref="IQueryable"/>, with optional filtering and including related properties.
        /// </summary>
        /// <param name="predicate">Optional filter for events.</param>
        /// <param name="includeProperties">Comma-separated list of related properties to include.</param>
        /// <returns>An <see cref="IQueryable"/> of events.</returns>
        public IQueryable<Event> GetAllEventsQuery(Expression<Func<Event, bool>>? predicate = null, string? includeProperties = null);

        /// <summary>
        /// Retrieves the first event that matches the specified criteria.
        /// </summary>
        /// <param name="predicate">Optional filter for events.</param>
        /// <param name="includeProperties">Comma-separated list of related properties to include.</param>
        /// <returns>A task representing the asynchronous operation, with the first event that matches the criteria.</returns>
        public Task<Event> GetFirstEvent(Expression<Func<Event, bool>>? predicate = null, string? includeProperties = null);

        /// <summary>
        /// Creates a new event based on the provided view model.
        /// </summary>
        /// <param name="eventCreateUpdateViewModel">The view model containing the event data to create.</param>
        /// <returns>A task representing the asynchronous operation, with the created event's view model.</returns>
        public Task<EventCreateUpdateViewModel> CreateEvent(EventCreateUpdateViewModel eventCreateUpdateViewModel);

        /// <summary>
        /// Updates an existing event based on the provided view model.
        /// </summary>
        /// <param name="eventCreateUpdateViewModel">The view model containing the event data to update.</param>
        /// <returns>A task representing the asynchronous operation, with the updated event's view model.</returns>
        public Task<EventCreateUpdateViewModel> UpdateEvent(EventCreateUpdateViewModel eventCreateUpdateViewModel);

        /// <summary>
        /// Deletes an event by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the event to delete.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
        public Task<bool> DeleteEvent(int id);

        /// <summary>
        /// Maps an <see cref="Event"/> entity to an <see cref="EventCreateUpdateViewModel"/> for updating.
        /// </summary>
        /// <param name="eventModel">The event entity to map.</param>
        /// <returns>A task representing the asynchronous operation, with the mapped view model.</returns>
        public Task<EventCreateUpdateViewModel> MapEventToUpdateViewModel(Event eventModel);
    }
}
