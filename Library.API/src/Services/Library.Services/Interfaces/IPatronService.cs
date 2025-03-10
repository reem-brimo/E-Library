using Library.SharedKernal.OperationResults;
using Library.Services.DTOs;
using System.Net;
using Library.Data.Models;

namespace Library.Services.Interfaces
{
    public interface IPatronService 
    {
        Task<OperationResult<HttpStatusCode, PatronResponseDto>> GetByIdAsync(int id);
        OperationResult<HttpStatusCode, IEnumerable<PatronResponseDto>> GetAll();
        Task<OperationResult<HttpStatusCode, bool>> AddAsync(PatronDto product);
        Task<OperationResult<HttpStatusCode, bool>> UpdateAsync(int id, PatronDto product);
        Task<OperationResult<HttpStatusCode, bool>> DeleteAsync(int id);
    }
}
