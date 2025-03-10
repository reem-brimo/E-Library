using Library.Data.Models;
using Library.Services.InfrastructureBases;

namespace Library.Services.Infrastructure
{
    public interface IBooksRepository : IGenericRepositoryAsync<Book>
    {

    }
}
