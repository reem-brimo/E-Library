using System.Net;
using System.Threading.Tasks;
using Library.App.Controllers;
using Library.Services.Interfaces;
using Library.SharedKernal.OperationResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Tests.Controllers
{
    public class BorrowingControllerTests
    {
        private readonly Mock<IBorrowingService> _mockBorrowingService;
        private readonly BorrowingController _borrowingController;

        public BorrowingControllerTests()
        {
            _mockBorrowingService = new Mock<IBorrowingService>();
            _borrowingController = new BorrowingController(_mockBorrowingService.Object);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturnOk_WhenBorrowingIsSuccessful()
        {
            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockBorrowingService.Setup(service => service.AddBorrowAsync(1, 1)).ReturnsAsync(operationResult);

            var result = await _borrowingController.BorrowBook(1, 1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact]
        public async Task BorrowBook_ShouldReturnInternalServerError_WhenBorrowingFails()
        {
            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                EnumResult = HttpStatusCode.InternalServerError,
            };
            operationResult.AddError("Borrowing failed");

            _mockBorrowingService.Setup(service => service.AddBorrowAsync(1, 1)).ReturnsAsync(operationResult);

            var result = await _borrowingController.BorrowBook(1, 1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(500, jsonResult.StatusCode);
            Assert.Equal("Borrowing failed", response!.Message);
        }

        [Fact]
        public async Task ReturnBook_ShouldReturnOk_WhenReturnIsSuccessful()
        {
            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockBorrowingService.Setup(service => service.UpdateBorrowAsync(1, 1)).ReturnsAsync(operationResult);

            var result = await _borrowingController.ReturnBook(1, 1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact]
        public async Task ReturnBook_ShouldReturnInternalServerError_WhenReturnFails()
        {
            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                EnumResult = HttpStatusCode.InternalServerError,
            };
            operationResult.AddError("Return failed");

            _mockBorrowingService.Setup(service => service.UpdateBorrowAsync(1, 1)).ReturnsAsync(operationResult);

            var result = await _borrowingController.ReturnBook(1, 1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(500, jsonResult.StatusCode);
            Assert.Equal("Return failed", response!.Message);
        }
    }
}