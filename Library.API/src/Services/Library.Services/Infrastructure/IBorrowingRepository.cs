using Library.Data.Models;
using Library.Services.InfrastructureBases;

namespace Library.Services.Infrastructure
{
    public interface IBorrowingRepository : IGenericRepositoryAsync<Borrow>

    {
        Task<Borrow> GetByBookAndPatron(int  BookId, int PatronId);
    }
}
