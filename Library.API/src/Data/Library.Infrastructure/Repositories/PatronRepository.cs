using Library.Data.Models;
using Library.Services.Infrastructure;

namespace Library.Infrastructure.Repositories
{
    public class PatronRepository : GenericRepositoryAsync<Patron>, IPatronRepository
    {
        private readonly ApplicationDbContext _context;

        public PatronRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }


}
