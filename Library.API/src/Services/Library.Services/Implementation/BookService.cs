using Library.Data.Models;
using Library.Services.DTOs;
using Library.Services.Infrastructure;
using Library.Services.Interfaces;
using Library.SharedKernal.OperationResults;
using Mapster;
using System.Net;

namespace Library.Services.Implementation
{
    public class BookService(IUnitOfWork unitOfWork) : IBookService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<OperationResult<HttpStatusCode, bool>> AddAsync(BookDto book)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            if (book == null)
            {
                result.EnumResult = HttpStatusCode.InternalServerError;
                result.AddError("Book can not be null!");
                return result;
            }

            var bookEntitiy = book.Adapt<Book>();

            await _unitOfWork.Books.AddAsync(bookEntitiy);

            await _unitOfWork.CommitAsync();

            result.EnumResult = HttpStatusCode.OK;
            result.Result = true;
            return result;

        }

        public virtual OperationResult<HttpStatusCode, IEnumerable<BookResponseDto>> GetAll()
        {
            var result = new OperationResult<HttpStatusCode, IEnumerable<BookResponseDto>>();

            var books = _unitOfWork.Books.GetTableNoTracking().Select(x => new BookResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Author=x.Author,
                ISBN = x.ISBN,
                PublicationYear = x.PublicationYear,
            });

            result.Result = books;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }

        public async Task<OperationResult<HttpStatusCode, BookResponseDto>> GetByIdAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, BookResponseDto>();

            if (id <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var bookEntity = await _unitOfWork.Books.GetByIdAsync(id);

            if (bookEntity == null)
            {
                result.AddError("Book Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            result.Result = bookEntity.Adapt<BookResponseDto>();
            result.EnumResult = HttpStatusCode.OK;

            return result;
        }
        
        public async Task<OperationResult<HttpStatusCode, bool>> UpdateAsync(int id, BookDto book)
        {
            var result = new OperationResult<HttpStatusCode, bool>();

            if (id <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var bookEntity = await _unitOfWork.Books.GetByIdAsync(id);

            if (bookEntity == null)
            {
                result.AddError("Book Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            bookEntity.Title = book.Title;
            bookEntity.Author = book.Author;
            bookEntity.ISBN = book.ISBN;
            bookEntity.PublicationYear = book.PublicationYear;

            await _unitOfWork.Books.UpdateAsync(bookEntity);

            await _unitOfWork.CommitAsync();

            result.EnumResult = HttpStatusCode.OK;
            result.Result = true;

            return result;
        }
        public async Task<OperationResult<HttpStatusCode, bool>> DeleteAsync(int id)
        {
            var result = new OperationResult<HttpStatusCode, bool>();
            if (id <= 0)
            {
                result.AddError("Id should be greater than 0");
                result.EnumResult = HttpStatusCode.BadRequest;
                return result;
            }

            var bookEntity = await _unitOfWork.Books.GetByIdAsync(id);


            if (bookEntity == null)
            {
                result.AddError("Book Not Found");
                result.EnumResult = HttpStatusCode.NotFound;
                return result;
            }

            await _unitOfWork.Books.DeleteAsync(bookEntity);
            await _unitOfWork.CommitAsync();


            result.Result = true;
            result.EnumResult = HttpStatusCode.OK;
            return result;
        }
    }

}
