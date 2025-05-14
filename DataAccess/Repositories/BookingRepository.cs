using DataAccess.DBContext;
using Models.IRepositories;
using Models.Models;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="Booking"/> entities.
    /// </summary>
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
