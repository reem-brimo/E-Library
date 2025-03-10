using Library.Data.Models;
using Library.Services.InfrastructureBases;

namespace Library.Services.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepositoryAsync<Book> Books { get; }
        IGenericRepositoryAsync<Patron> Patrons { get; }
        IBorrowingRepository BorrowingRecords { get; }
        Task<int> CommitAsync();
    }
}
