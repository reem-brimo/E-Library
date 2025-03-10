using Library.Data.Models;
using Library.Services.Implementation;
using Library.Services.Infrastructure;
using Library.Services.Interfaces;
using Library.SharedKernal.OperationResults;
using Moq;
using System.Net;
using Xunit;

namespace Tests.Services
{

    public class BorrowingServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BorrowingService _borrowingService;

        public BorrowingServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _borrowingService = new BorrowingService(_mockUnitOfWork.Object);
        }

        // Test for AddBorrowAsync
        [Fact]
        public async Task AddBorrowAsync_ShouldReturnOk_WhenBookAndPatronExist()
        {

            var book = new Book { Id = 1, Title = "Test Book" };
            var patron = new Patron { Id = 1, FirstName = "John", LastName = "Doe" };

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync(patron);
            _mockUnitOfWork.Setup(uow => uow.BorrowingRecords.AddAsync(It.IsAny<Borrow>())).ReturnsAsync(new Borrow());
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);


            var result = await _borrowingService.AddBorrowAsync(1, 1);


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
        }

        [Fact]

        public async Task AddBorrowAsync_ShouldReturnNotFound_WhenBookDoesNotExist()
        {

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync((Book)null);
            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync((Patron)null);


            var result = await _borrowingService.AddBorrowAsync(1, 1);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Book not found", result.ErrorMessages);
        }

        [Fact]
        public async Task AddBorrowAsync_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {

            var book = new Book { Id = 1, Title = "Test Book" };
            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync((Patron)null);


            var result = await _borrowingService.AddBorrowAsync(1, 1);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("patron not found", result.ErrorMessages);
        }

        // Test for UpdateBorrowAsync
        [Fact]
        public async Task UpdateBorrowAsync_ShouldReturnOk_WhenBorrowedBookExists()
        {

            var book = new Book { Id = 1, Title = "Test Book" };
            var patron = new Patron { Id = 1, FirstName = "John", LastName = "Doe" };
            var borrowedEntity = new Borrow { BookId = 1, PatronId = 1, BorrowedDate = DateTime.UtcNow };

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync(patron);
            _mockUnitOfWork.Setup(uow => uow.BorrowingRecords.GetByBookAndPatron(1, 1)).ReturnsAsync(borrowedEntity);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);


            var result = await _borrowingService.UpdateBorrowAsync(1, 1);


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
            Assert.NotNull(borrowedEntity.ReturnedDate);
        }

        [Fact]
        public async Task UpdateBorrowAsync_ShouldReturnNotFound_WhenBookDoesNotExist()
        {

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync((Book)null);
            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync((Patron)null);


            var result = await _borrowingService.UpdateBorrowAsync(1, 1);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Book not found", result.ErrorMessages);
        }

        [Fact]
        public async Task UpdateBorrowAsync_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {

            var book = new Book { Id = 1, Title = "Test Book" };
            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync((Patron)null);


            var result = await _borrowingService.UpdateBorrowAsync(1, 1);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("patron not found", result.ErrorMessages);
        }

        [Fact]
        public async Task UpdateBorrowAsync_ShouldReturnNotFound_WhenBorrowedBookDoesNotExist()
        {

            var book = new Book { Id = 1, Title = "Test Book" };
            var patron = new Patron { Id = 1, FirstName = "John", LastName = "Doe" };

            _mockUnitOfWork.Setup(uow => uow.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync(patron);
            _mockUnitOfWork.Setup(uow => uow.BorrowingRecords.GetByBookAndPatron(1, 1)).ReturnsAsync((Borrow)null);


            var result = await _borrowingService.UpdateBorrowAsync(1, 1);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Borrowed book not found", result.ErrorMessages);
        }
    }
}