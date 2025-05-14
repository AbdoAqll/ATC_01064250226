using BusinessLogic.IServices;
using Models.IRepositories;
using Models.Models;
using Models.ViewModels;
using System.Linq.Expressions;

namespace BusinessLogic.Services
{
    /// <summary>
    /// The service responsible for managing events, including creating, updating, deleting, and retrieving event information.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work instance to manage transactions.</param>
        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="model">The view model containing event details to create the event.</param>
        /// <returns>A task representing the asynchronous operation, with the created event view model.</returns>
        public async Task<EventCreateUpdateViewModel> CreateEvent(EventCreateUpdateViewModel model)
        {
            var eventEntity = MapToEventEntity(model);
            await _unitOfWork.EventRepository.AddAsync(eventEntity);
            await _unitOfWork.SaveChangesAsync();

            model.Id = eventEntity.Id;
            return model;
        }

        /// <summary>
        /// Updates an existing event.
        /// </summary>
        /// <param name="model">The view model containing updated event details.</param>
        /// <returns>A task representing the asynchronous operation, with the updated event view model.</returns>
        /// <exception cref="Exception">Thrown when the event to update is not found.</exception>
        public async Task<EventCreateUpdateViewModel> UpdateEvent(EventCreateUpdateViewModel model)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetFirstOrDefaultAsync(e => e.Id == model.Id, "EventTranslations");

            if (existingEvent == null)
                throw new Exception($"Event with ID {model.Id} not found.");

            // Update main properties
            existingEvent.Date = model.Date;
            existingEvent.Price = model.Price;
            existingEvent.GoogleMapUrl = model.GoogleMapUrl;
            existingEvent.ImageUrl = model.ImageUrl;

            // Remove old translations
            existingEvent.EventTranslations.Clear();

            // Add updated translations
            foreach (var translation in model.EventTranslations)
            {
                existingEvent.EventTranslations.Add(new EventTranslations
                {
                    LanguageCode = translation.LanguageCode,
                    Name = translation.Name,
                    Category = translation.Category,
                    Venue = translation.Venue,
                    Description = translation.Description,
                    EventTags = translation.EventTags
                });
            }

            await _unitOfWork.EventRepository.UpdateAsync(existingEvent);
            await _unitOfWork.SaveChangesAsync();

            return model;
        }

        /// <summary>
        /// Deletes an event by its ID.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating success.</returns>
        public async Task<bool> DeleteEvent(int id)
        {
            var existingEvent = await _unitOfWork.EventRepository.GetFirstOrDefaultAsync(e => e.Id == id);
            if (existingEvent == null) return false;

            await _unitOfWork.EventRepository.HardRemoveAsync(existingEvent);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Retrieves all events with optional filtering, pagination, and include properties.
        /// </summary>
        /// <param name="predicate">An optional expression to filter events.</param>
        /// <param name="includeProperties">A comma-separated string of related entities to include in the result.</param>
        /// <param name="pageNumber">An optional page number for pagination.</param>
        /// <param name="pageSize">An optional page size for pagination.</param>
        /// <returns>A task representing the asynchronous operation, with a list of events.</returns>
        public async Task<IEnumerable<Event>> GetAllEvents(Expression<Func<Event, bool>>? predicate = null, string? includeProperties = null, int? pageNumber = null, int? pageSize = null)
        {
            return await _unitOfWork.EventRepository.GetAllAsync(predicate, includeProperties, pageNumber, pageSize);
        }

        /// <summary>
        /// Retrieves the first event that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">An optional expression to filter events.</param>
        /// <param name="includeProperties">A comma-separated string of related entities to include in the result.</param>
        /// <returns>A task representing the asynchronous operation, with the first matching event.</returns>
        public async Task<Event> GetFirstEvent(Expression<Func<Event, bool>>? predicate = null, string? includeProperties = null)
        {
            return await _unitOfWork.EventRepository.GetFirstOrDefaultAsync(predicate, includeProperties);
        }

        /// <summary>
        /// Maps an event entity to a view model for updating.
        /// </summary>
        /// <param name="eventModel">The event entity to map.</param>
        /// <returns>A task representing the asynchronous operation, with the mapped view model.</returns>
        public Task<EventCreateUpdateViewModel> MapEventToUpdateViewModel(Event eventModel)
        {
            var viewModel = new EventCreateUpdateViewModel
            {
                Id = eventModel.Id,
                Date = eventModel.Date,
                Price = eventModel.Price,
                GoogleMapUrl = eventModel.GoogleMapUrl,
                ImageUrl = eventModel.ImageUrl,
                EventTranslations = eventModel.EventTranslations.Select(t => new EventCreateUpdateViewModel.EventTranslationViewModel
                {
                    Id = t.Id,
                    LanguageCode = t.LanguageCode,
                    Name = t.Name,
                    Category = t.Category,
                    Venue = t.Venue,
                    Description = t.Description,
                    EventTags = t.EventTags
                }).ToList()
            };
            return Task.FromResult(viewModel);
        }

        /// <summary>
        /// Retrieves an IQueryable of events that can be further customized with predicates and include properties.
        /// </summary>
        /// <param name="predicate">An optional expression to filter events.</param>
        /// <param name="includeProperties">A comma-separated string of related entities to include in the result.</param>
        /// <returns>An IQueryable of events.</returns>
        public IQueryable<Event> GetAllEventsQuery(Expression<Func<Event, bool>>? predicate = null, string? includeProperties = null)
        {
            return _unitOfWork.EventRepository.GetAllQuery(predicate, includeProperties);
        }

        /// <summary>
        /// Maps an event view model to an entity.
        /// </summary>
        /// <param name="model">The event view model to map.</param>
        /// <returns>The mapped event entity.</returns>
        private Event MapToEventEntity(EventCreateUpdateViewModel model)
        {
            return new Event
            {
                Date = model.Date,
                Price = model.Price,
                GoogleMapUrl = model.GoogleMapUrl,
                ImageUrl = model.ImageUrl,
                EventTranslations = model.EventTranslations.Select(t => new EventTranslations
                {
                    LanguageCode = t.LanguageCode,
                    Name = t.Name,
                    Category = t.Category,
                    Venue = t.Venue,
                    Description = t.Description,
                    EventTags = t.EventTags
                }).ToList()
            };
        }
    }
}
