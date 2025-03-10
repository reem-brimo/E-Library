using Library.Data.Models;
using Library.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories
{
    public class BorrowingRepository : GenericRepositoryAsync<Borrow>, IBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Borrow> GetByBookAndPatron(int BookId, int PatronId)
        {
            return await _context.BorrowingRecords
                            .FirstOrDefaultAsync(x => x.BookId == BookId && x.PatronId == PatronId);
        }
    }
}
