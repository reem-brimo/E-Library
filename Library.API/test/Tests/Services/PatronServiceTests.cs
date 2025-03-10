using Library.Data.Models;
using Library.Services.DTOs;
using Library.Services.Implementation;
using Library.Services.Infrastructure;
using Library.Services.Interfaces;
using Library.SharedKernal.OperationResults;
using Mapster;
using Moq;
using System.Net;
using Xunit;

namespace Tests.Services
{
    public class PatronServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly PatronService _patronService;

        public PatronServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _patronService = new PatronService(_mockUnitOfWork.Object);
        }

        // Test for GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatron_WhenPatronExists()
        {

            var patronEntity = new Patron
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = true
            };

            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync(patronEntity);


            var result = await _patronService.GetByIdAsync(1);


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.Equal("John", result.Result.FirstName);
            Assert.Equal("Doe", result.Result.LastName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {

            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync((Patron)null);


            var result = await _patronService.GetByIdAsync(1);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Patron Not Found", result.ErrorMessages);
        }

        // Test for GetAll
        [Fact]
        public void GetAll_ShouldReturnPatrons()
        {

            var patrons = new List<Patron>
        {
            new Patron { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
            new Patron { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
        };

            _mockUnitOfWork.Setup(uow => uow.Patrons.GetTableNoTracking()).Returns(patrons.AsQueryable());


            var result = _patronService.GetAll();


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.Equal(2, result.Result.Count());
        }

        // Test for AddAsync
        [Fact]
        public async Task AddAsync_ShouldReturnOk_WhenPatronIsValid()
        {

            var patronDto = new PatronDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = true
            };

            var patronEntity = patronDto.Adapt<Patron>();

            _mockUnitOfWork.Setup(uow => uow.Patrons.AddAsync(It.IsAny<Patron>())).ReturnsAsync(patronEntity);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);


            var result = await _patronService.AddAsync(patronDto);


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnInternalServerError_WhenPatronIsNull()
        {

            PatronDto patronDto = null;


            var result = await _patronService.AddAsync(patronDto);


            Assert.Equal(HttpStatusCode.InternalServerError, result.EnumResult);
            Assert.Contains("Patron can not be null!", result.ErrorMessages);
        }

        // Test for UpdateAsync
        [Fact]
        public async Task UpdateAsync_ShouldReturnOk_WhenPatronExists()
        {

            var patronEntity = new Patron
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = true
            };

            var patronDto = new PatronDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                PhoneNumber = "987-654-3210",
                DateOfBirth = new DateTime(1995, 5, 5),
                Address = "456 Elm St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = false
            };

            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync(patronEntity);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);


            var result = await _patronService.UpdateAsync(1, patronDto);


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
            Assert.Equal("Jane", patronEntity.FirstName);
            Assert.Equal("Doe", patronEntity.LastName);
            Assert.Equal("jane.doe@example.com", patronEntity.Email);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {

            var patronDto = new PatronDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                PhoneNumber = "987-654-3210",
                DateOfBirth = new DateTime(1995, 5, 5),
                Address = "456 Elm St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = false
            };

            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync((Patron)null);


            var result = await _patronService.UpdateAsync(1, patronDto);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Patron Not Found", result.ErrorMessages);
        }

        // Test for DeleteAsync
        [Fact]
        public async Task DeleteAsync_ShouldReturnOk_WhenPatronExists()
        {

            var patronEntity = new Patron
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890",
                DateOfBirth = new DateTime(1990, 1, 1),
                Address = "123 Main St",
                MembershipStartDate = DateTime.UtcNow,
                IsActive = true
            };

            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync(patronEntity);
            _mockUnitOfWork.Setup(uow => uow.CommitAsync()).ReturnsAsync(1);


            var result = await _patronService.DeleteAsync(1);


            Assert.Equal(HttpStatusCode.OK, result.EnumResult);
            Assert.True(result.Result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenPatronDoesNotExist()
        {

            _mockUnitOfWork.Setup(uow => uow.Patrons.GetByIdAsync(1)).ReturnsAsync((Patron)null);


            var result = await _patronService.DeleteAsync(1);


            Assert.Equal(HttpStatusCode.NotFound, result.EnumResult);
            Assert.Contains("Patron Not Found", result.ErrorMessages);
        }
    }
}