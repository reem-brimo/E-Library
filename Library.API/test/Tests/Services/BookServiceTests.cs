using Library.Data.Models;
using Library.Services.DTOs;
using Library.Services.Implementation;
using Library.Services.Infrastructure;
using Mapster;
using Moq;
using System.Net;

namespace Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _bookService = new BookService(_mockUnitOfWork.Object);
        }

        #region Create
        // Test for AddAsync
        [Fact]
        public async Task AddAsync_ShouldReturnOk_WhenBookIsValid()
        {

            var bookDto = new BookDto { Title = "Test Book", Author = "Test Author", ISBN = "1234567890", PublicationYear = 2023 };
            var bookEntity = bookDto.Adapt<Book>();

            _mockUnitOfWork.Setup(uow => uow.Books.AddAsync(It.IsAny<Book>())).ReturnsAsync(bookEntity);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);


            var result = await _bookService.AddAsync(bookDto);


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInternalServerError_WhenBookIsNull()
        {
            BookDto bookDto = null!;

            var result = await _bookService.AddAsync(bookDto!);

            Assert.Equal(HttpStatusCode.InternalServerError, result.EnumResult);
            Assert.Contains("Book can not be null!", result.ErrorMessages);
        }
        #endregion

        #region GetAll
        [Fact]
        public void GetAll_ShouldReturnBooks()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", ISBN = "1234567890", PublicationYear = 2021 },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", ISBN = "0987654321", PublicationYear = 2022 }
            };

            _mockUnitOfWork.Setup(uow => uow.Books.GetTableNoTracking()).Returns(books.AsQueryable());

            var result = _bookService.GetAll();
            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.Equal(2, result.Result.Count());
        }
        #endregion

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists()
        {
            var bookEntity = new Book { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "1234567890", PublicationYear = 2023 };
            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(bookEntity);

            var result = await _bookService.GetByIdAsync(1);

            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.Equal("Test Book", result.Result.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1))!.ReturnsAsync((Book)null!);

            var result = await _bookService.GetByIdAsync(1);

            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Book Not Found", result.ErrorMessages);
        }
        #endregion

        #region UpdateAsync
        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_WhenBookExists()
        {
            var bookEntity = new Book { Id = 1, Title = "Old Title", Author = "Old Author", ISBN = "1234567890", PublicationYear = 2021 };
            var bookDto = new BookDto { Title = "New Title", Author = "New Author", ISBN = "0987654321", PublicationYear = 2023 };

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(bookEntity);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);

            var result = await _bookService.UpdateAsync(1, bookDto);

            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
            Assert.Equal("New Title", bookEntity.Title);
            Assert.Equal("New Author", bookEntity.Author);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            var bookDto = new BookDto { Title = "New Title", Author = "New Author", ISBN = "0987654321", PublicationYear = 2023 };
            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1))!.ReturnsAsync((Book)null!);

            var result = await _bookService.UpdateAsync(1, bookDto);

            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Book Not Found", result.ErrorMessages);
        }
        #endregion

        #region DeleteAsync
        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_WhenBookExists()
        {
            var bookEntity = new Book { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "1234567890", PublicationYear = 2023 };
            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(bookEntity);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);


            var result = await _bookService.DeleteAsync(1);

            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenBookDoesNotExist()
        {
            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1))!.ReturnsAsync((Book)null!);

            var result = await _bookService.DeleteAsync(1);

            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Book Not Found", result.ErrorMessages);
        }
        #endregion

    }

}