using Library.Data.Models;
using Library.Services.Infrastructure;
using Library.Services.Interfaces;
using Library.SharedKernal.OperationResults;
using System.Net;

namespace Library.Services.Implementation
{
    public class BorrowingService(IUnitOfWork unitOfWork) : IBorrowingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public async Task<OperationResult<HttpStatusCode, bool>> AddBorrowAsync(int bookId, int patronId)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            if (bookId <= 0 || patronId <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var book = await _unitOfWork.Books.GetByIdAsync(bookId);
            var patron = await _unitOfWork.Patrons.GetByIdAsync(patronId);

            if (book == null)
            {
                result.EnumResult = HttpStatusCode.NotFound;
                result.AddError("Book not found");
                return result;
            }
            if (patron == null)
            {
                result.EnumResult = HttpStatusCode.NotFound;
                result.AddError("patron not found");
                return result;
            }


            await _unitOfWork.BorrowingRecords.AddAsync(new Borrow 
                                                            {   BookId = bookId,
                                                                PatronId = patronId,
                                                                BorrowedDate = DateTime.UtcNow });

            await _unitOfWork.CommitAsync();

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public async Task<OperationResult<HttpStatusCode, bool>> UpdateBorrowAsync(int bookId, int patronId)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            if (bookId <= 0 || patronId <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var book = await _unitOfWork.Books.GetByIdAsync(bookId);
            var patron = await _unitOfWork.Patrons.GetByIdAsync(patronId);

            if (book == null)
            {
                result.EnumResult = HttpStatusCode.NotFound;
                result.AddError("Book not found");
                return result;
            }
            if (patron == null)
            {
                result.EnumResult = HttpStatusCode.NotFound;
                result.AddError("patron not found");
                return result;
            }

            var borrowedEntity = await _unitOfWork.BorrowingRecords.GetByBookAndPatron(bookId, patronId);

            if (borrowedEntity == null)
            {
                result.EnumResult = HttpStatusCode.NotFound;
                result.AddError("Borrowed book not found");
                return result;
            }

            borrowedEntity.ReturnedDate = DateTime.UtcNow;

            await _unitOfWork.BorrowingRecords.UpdateAsync(borrowedEntity);

            await _unitOfWork.CommitAsync();

            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }
    }
}
