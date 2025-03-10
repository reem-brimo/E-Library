using Library.App.Controllers;
using Library.Services.DTOs;
using Library.Services.Interfaces;
using Library.Services.Validators;
using Library.SharedKernal.OperationResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BooksController _booksController;

        public BooksControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _booksController = new BooksController(_mockBookService.Object);
        }

        #region Get
        // Test for GET /api/books
        [Fact]
        public void GetBooks_ShouldReturnOk_WhenBooksExist()
        {
            var books = new List<BookResponseDto>
                {
                    new BookResponseDto { Id = 1, Title = "Book 1", Author = "Author 1" },
                    new BookResponseDto { Id = 2, Title = "Book 2", Author = "Author 2" }
                };

            var operationResult = new OperationResult<HttpStatusCode, IEnumerable<BookResponseDto>>
            {
                Result = books,
                EnumResult = HttpStatusCode.OK
            };

            _mockBookService.Setup(service => service.GetAll()).Returns(operationResult);


            var result = _booksController.GetBooks();


            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.Equal(books, jsonResult.Value);
        }

        // Test for GET /api/books/{id}
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenBookExists()
        {
            var book = new BookResponseDto { Id = 1, Title = "Test Book", Author = "Test Author" };


            var operationResult = new OperationResult<HttpStatusCode, BookResponseDto>
            {
                Result = book,
                EnumResult = HttpStatusCode.OK
            };

            _mockBookService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(operationResult);


            var result = await _booksController.GetById(1);


            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.Equal(book, jsonResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenBookDoesNotExist()
        {

            var operationResult = new OperationResult<HttpStatusCode, BookResponseDto>
            {
                EnumResult = HttpStatusCode.NotFound,
            };
            operationResult.AddError("Book Not Found");

            _mockBookService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(operationResult);


            var result = await _booksController.GetById(1);


            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(404, jsonResult.StatusCode);
            Assert.Equal("Book Not Found", response!.Message);
        }
        #endregion

        #region Post

        // Test for POST /api/books
        [Fact]
        public async Task Create_ShouldReturnOk_WhenBookIsValid()
        {
            var bookDto = new BookDto { Title = "Test Book" ,Author = "Author", ISBN = "1234567890", PublicationYear = 2023 } ;

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockBookService.Setup(service => service.AddAsync(bookDto)).ReturnsAsync(operationResult);


            var result = await _booksController.Create(bookDto);

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value!);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenBookIsInvalid()
        {
            var bookDto = new BookDto { Title = "Test Book", ISBN = "1234567890", PublicationYear = 2023 };

            var validator = new BookDtoValidator();
            var validationResult = await validator.ValidateAsync(bookDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _booksController.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            var result = await _booksController.Create(bookDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var serializableError = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(serializableError.ContainsKey("Author"));
            Assert.Equal("Author is required.", ((string[])serializableError["Author"])[0]);
        }
        #endregion

        #region Put

        // Test for PUT /api/books/{id}
        [Fact]
        public async Task Update_ShouldReturnOk_WhenBookExists()
        {

            var bookDto = new BookDto { Title = "Test Book", Author = "Author", ISBN = "1234567890", PublicationYear = 2023 };

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockBookService.Setup(service => service.UpdateAsync(1, bookDto)).ReturnsAsync(operationResult);


            var result = await _booksController.Update(1, bookDto);


            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value!);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenBookDoesNotExist()
        {

            var bookDto = new BookDto { Title = "Test Book", Author = "Author", ISBN = "1234567890", PublicationYear = 2023 };

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                EnumResult = HttpStatusCode.NotFound,
            };
            operationResult.AddError("Book Not Found");

            _mockBookService.Setup(service => service.UpdateAsync(1, bookDto)).ReturnsAsync(operationResult);


            var result = await _booksController.Update(1, bookDto);


            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(404, jsonResult.StatusCode);
            Assert.Equal("Book Not Found", response!.Message);
        }


        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenAuthorIsMissing()
        {
            var bookDto = new BookDto { Title = "Test Book", ISBN = "1234567890", PublicationYear = 2023 }; // Missing Author


            var validator = new BookDtoValidator();
            var validationResult = await validator.ValidateAsync(bookDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _booksController.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            var result = await _booksController.Update(1,bookDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var serializableError = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(serializableError.ContainsKey("Author"));
            Assert.Equal("Author is required.", ((string[])serializableError["Author"])[0]);
        }
        #endregion

        #region Delete
        // Test for DELETE /api/books/{id}
        [Fact]
        public async Task DeleteBooks_ShouldReturnOk_WhenBookExists()
        {

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockBookService.Setup(service => service.DeleteAsync(1)).ReturnsAsync(operationResult);


            var result = await _booksController.DeleteBooks(1);


            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact]
        public async Task DeleteBooks_ShouldReturnNotFound_WhenBookDoesNotExist()
        {

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                EnumResult = HttpStatusCode.NotFound,
            };
            operationResult.AddError("Book Not Found");

            _mockBookService.Setup(service => service.DeleteAsync(1)).ReturnsAsync(operationResult);


            var result = await _booksController.DeleteBooks(1);


            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(404, jsonResult.StatusCode);
            Assert.Equal("Book Not Found", response!.Message);
        }
        #endregion
    }
}


