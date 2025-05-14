using DataAccess.DBContext;
using Models.IRepositories;
using Models.Models;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="Event"/> entities.
    /// </summary>
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public EventRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
