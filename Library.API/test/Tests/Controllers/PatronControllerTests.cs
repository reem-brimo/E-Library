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

    public class PatronsControllerTests
    {
        private readonly Mock<IPatronService> _mockPatronService;
        private readonly PatronsController _patronsController;

        public PatronsControllerTests()
        {
            _mockPatronService = new Mock<IPatronService>();
            _patronsController = new PatronsController(_mockPatronService.Object);
        }

        #region Get
        // Test for GET /api/patrons
        [Fact]
        public void GetPatrons_ShouldReturnOk_WhenPatronsExist()
        {
            var patrons = new List<PatronResponseDto>
        {
            new PatronResponseDto { Id = 1, FirstName = "John", LastName = "Doe" },
            new PatronResponseDto { Id = 2, FirstName = "Jane", LastName = "Doe" }
        };

            var operationResult = new OperationResult<HttpStatusCode, IEnumerable<PatronResponseDto>>
            {
                Result = patrons,
                EnumResult = HttpStatusCode.OK
            };

            _mockPatronService.Setup(service => service.GetAll()).Returns(operationResult);

            var result = _patronsController.GetPatrons();

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.Equal(patrons, jsonResult.Value);
        }

        // Test for GET /api/patrons/{id}
        [Fact]
        public async Task GetById_ShouldReturnOk_WhenPatronExists()
        {
            var patron = new PatronResponseDto { Id = 1, FirstName = "John", LastName = "Doe" };

            var operationResult = new OperationResult<HttpStatusCode, PatronResponseDto>
            {
                Result = patron,
                EnumResult = HttpStatusCode.OK
            };

            _mockPatronService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(operationResult);

            var result = await _patronsController.GetById(1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.Equal(patron, jsonResult.Value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {
            var operationResult = new OperationResult<HttpStatusCode, PatronResponseDto>
            {
                EnumResult = HttpStatusCode.NotFound,
            };
            operationResult.AddError("Patron Not Found");

            _mockPatronService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(operationResult);

            var result = await _patronsController.GetById(1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(404, jsonResult.StatusCode);
            Assert.Equal("Patron Not Found", response.Message);
        }
        #endregion

        #region Post
        // Test for POST /api/patrons
        [Fact]
        public async Task Create_ShouldReturnOk_WhenPatronIsValid()
        {
            var patronDto = new PatronDto { FirstName = "John", LastName = "Doe" };

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockPatronService.Setup(service => service.AddAsync(patronDto)).ReturnsAsync(operationResult);

            var result = await _patronsController.Create(patronDto);

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenPatronIsInvalid()
        {
            var patronDto = new PatronDto
            {
                FirstName = "",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = true
            };

            var validator = new PatronDtoValidator();
            var validationResult = await validator.ValidateAsync(patronDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _patronsController.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            var result = await _patronsController.Create(patronDto);

            
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var serializableError = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(serializableError.ContainsKey("FirstName"));
            Assert.Equal("First name is required.", ((string[])serializableError["FirstName"])[0]);
        }
        #endregion

        #region Put
        // Test for PUT /api/patrons/{id}
        [Fact]
        public async Task Update_ShouldReturnOk_WhenPatronExists()
        {
            var patronDto = new PatronDto { FirstName = "John", LastName = "Doe" };

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockPatronService.Setup(service => service.UpdateAsync(1, patronDto)).ReturnsAsync(operationResult);

            var result = await _patronsController.Update(1, patronDto);

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenPatronIsInvalid()
        {
            var patronDto = new PatronDto
            {
                FirstName = "",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = true
            };

            var validator = new PatronDtoValidator();
            var validationResult = await validator.ValidateAsync(patronDto);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _patronsController.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            var result = await _patronsController.Update(1, patronDto);


            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var serializableError = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(serializableError.ContainsKey("FirstName"));
            Assert.Equal("First name is required.", ((string[])serializableError["FirstName"])[0]);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {
            var patronDto = new PatronDto { FirstName = "John", LastName = "Doe" };

            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                EnumResult = HttpStatusCode.NotFound,
            };
            operationResult.AddError("Patron Not Found");

            _mockPatronService.Setup(service => service.UpdateAsync(1, patronDto)).ReturnsAsync(operationResult);

            var result = await _patronsController.Update(1, patronDto);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(404, jsonResult.StatusCode);
            Assert.Equal("Patron Not Found", (response!.Message));
        }

        #endregion

        #region Delete
        // Test for DELETE /api/patrons/{id}
        [Fact]
        public async Task DeletePatron_ShouldReturnOk_WhenPatronExists()
        {
            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                Result = true,
                EnumResult = HttpStatusCode.OK
            };

            _mockPatronService.Setup(service => service.DeleteAsync(1)).ReturnsAsync(operationResult);

            var result = await _patronsController.DeletePatron(1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            Assert.Equal(200, jsonResult.StatusCode);
            Assert.True((bool)jsonResult.Value);
        }

        [Fact]
        public async Task DeletePatron_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {
            var operationResult = new OperationResult<HttpStatusCode, bool>
            {
                EnumResult = HttpStatusCode.NotFound,
             };
            operationResult.AddError("Patron Not Found");

            _mockPatronService.Setup(service => service.DeleteAsync(1)).ReturnsAsync(operationResult);

            var result = await _patronsController.DeletePatron(1);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var response = JsonConvert.DeserializeObject<ApiResponse>(JsonConvert.SerializeObject(jsonResult.Value));

            Assert.Equal(404, jsonResult.StatusCode);
            Assert.Equal("Patron Not Found", response!.Message);
        }
        #endregion

    }
}
