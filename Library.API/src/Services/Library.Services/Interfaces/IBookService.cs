using Library.Services.DTOs;
using Library.SharedKernal.OperationResults;
using System.Net;

namespace Library.Services.Interfaces
{

    public interface IBookService
    {
        Task<OperationResult<HttpStatusCode, BookResponseDto>> GetByIdAsync(int id);

        OperationResult<HttpStatusCode, IEnumerable<BookResponseDto>> GetAll();

        Task<OperationResult<HttpStatusCode, bool>> AddAsync(BookDto book);

        Task<OperationResult<HttpStatusCode, bool>> UpdateAsync(int id, BookDto book);

        Task<OperationResult<HttpStatusCode, bool>> DeleteAsync(int id);
    }
}

