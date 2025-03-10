using Library.Data.Models;
using Library.Services.Infrastructure;

namespace Library.Infrastructure.Repositories
{
    public class BooksRepository(ApplicationDbContext dbContext) : GenericRepositoryAsync<Book>(dbContext), IBooksRepository
    {
        private readonly ApplicationDbContext _context = dbContext;
    }
}
