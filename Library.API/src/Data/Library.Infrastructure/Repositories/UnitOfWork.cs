using Library.Data.Models;
using Library.Services.Infrastructure;
using Library.Services.InfrastructureBases;

namespace Library.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Books = new GenericRepositoryAsync<Book>(_context);
            Patrons = new GenericRepositoryAsync<Patron>(_context);
            BorrowingRecords = new BorrowingRepository(_context);
        }

        public IGenericRepositoryAsync<Book> Books { get; }
        public IGenericRepositoryAsync<Patron> Patrons { get; }
        public IBorrowingRepository BorrowingRecords { get; }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
