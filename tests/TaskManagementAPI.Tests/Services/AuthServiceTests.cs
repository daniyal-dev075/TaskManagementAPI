using Moq;
using FluentAssertions;
using TaskManagementAPI.Application.Services;
using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.Application.Interfaces.Repositories;
using TaskManagementAPI.Application.DTOs.Auth;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Domain.Enums;
using System.Linq.Expressions;

namespace TaskManagementAPI.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IGenericRepository<User>> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IGenericRepository<User>>();
            _tokenServiceMock = new Mock<ITokenService>();
            _authService = new AuthService(_userRepositoryMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_ReturnsAuthResponse_WhenUserIsNew()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User>());

            _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _userRepositoryMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            _tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns("fake-jwt-token");

            // Act
            var result = await _authService.RegisterAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-jwt-token");
            result.Email.Should().Be("john@example.com");
            result.Role.Should().Be("Member");
        }

        [Fact]
        public async Task RegisterAsync_ThrowsException_WhenEmailAlreadyExists()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Password = "password123"
            };

            _userRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { new User { Email = "john@example.com" } });

            // Act
            var act = async () => await _authService.RegisterAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Email is already registered.");
        }

        [Fact]
        public async Task LoginAsync_ReturnsAuthResponse_WhenCredentialsAreValid()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "john@example.com",
                Password = "password123"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
            var user = new User
            {
                Id = 1,
                FullName = "John Doe",
                Email = "john@example.com",
                PasswordHash = hashedPassword,
                Role = UserRole.Member
            };

            _userRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            _tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns("fake-jwt-token");

            // Act
            var result = await _authService.LoginAsync(dto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-jwt-token");
            result.Email.Should().Be("john@example.com");
        }

        [Fact]
        public async Task LoginAsync_ThrowsException_WhenPasswordIsInvalid()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "john@example.com",
                Password = "wrongpassword"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
            var user = new User
            {
                Id = 1,
                Email = "john@example.com",
                PasswordHash = hashedPassword,
                Role = UserRole.Member
            };

            _userRepositoryMock
                .Setup(r => r.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(new List<User> { user });

            // Act
            var act = async () => await _authService.LoginAsync(dto);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid email or password.");
        }
    }
}