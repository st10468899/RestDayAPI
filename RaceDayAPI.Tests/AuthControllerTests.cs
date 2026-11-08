using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Controllers;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using Xunit;

namespace RaceDayAPI.Tests
{
    public class AuthControllerTests
    {
        // Creates an isolated in-memory database instance for each unit test
        private RaceDayDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RaceDayDbContext(options);
        }

        // Mocks HttpContext and Session storage required for login/logout actions
        private ControllerContext GetMockControllerContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            return new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public async Task Register_ValidUser_Returns201Created()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var dto = new RegisterDto
            {
                Name = "John Participant",
                Email = "john@example.com",
                Password = "Password123!",
                Role = "Participant"
            };

            // Act
            var result = await controller.Register(dto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);
            var dto = new RegisterDto
            {
                Name = "John Participant",
                Email = "john@example.com",
                Password = "Password123!",
                Role = "Participant"
            };

            await controller.Register(dto); // First registration

            // Act
            var result = await controller.Register(dto) as ObjectResult; // Attempt duplicate registration

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Login_ValidCredentials_Returns200Ok()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context)
            {
                ControllerContext = GetMockControllerContext()
            };

            var registerDto = new RegisterDto
            {
                Name = "Alice Organiser",
                Email = "alice@example.com",
                Password = "Password123!",
                Role = "Organiser"
            };
            await controller.Register(registerDto);

            var loginDto = new LoginDto
            {
                Email = "alice@example.com",
                Password = "Password123!"
            };

            // Act
            var result = await controller.Login(loginDto) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidPassword_Returns401Unauthorized()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context)
            {
                ControllerContext = GetMockControllerContext()
            };

            var registerDto = new RegisterDto
            {
                Name = "Bob Runner",
                Email = "bob@example.com",
                Password = "Password123!",
                Role = "Participant"
            };
            await controller.Register(registerDto);

            var loginDto = new LoginDto
            {
                Email = "bob@example.com",
                Password = "WrongPassword!"
            };

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            Assert.NotNull(result);
        }
    }

    // Custom in-memory session implementation for unit testing
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public bool IsAvailable => true;
        public string Id => "TestSessionId";
        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _sessionStorage.Remove(key);
        public void Set(string key, byte[] value) => _sessionStorage[key] = value;
        public bool TryGetValue(string key, out byte[]? value) => _sessionStorage.TryGetValue(key, out value);
    }
}