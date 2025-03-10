using Library.Services.DTOs;
using Library.SharedKernal.OperationResults;
using System.Net;

namespace Library.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<OperationResult<HttpStatusCode, bool>> AddBorrowAsync(int bookId, int patronId);
        Task<OperationResult<HttpStatusCode, bool>> UpdateBorrowAsync(int bookId, int patronId);
    }
}
