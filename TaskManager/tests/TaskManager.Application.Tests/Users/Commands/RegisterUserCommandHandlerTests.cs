using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Users.Commands.Register;
using TaskManager.Domain.Entities;
using Xunit;

namespace TaskManager.Application.Tests.Users.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly Mock<IJwtTokenService> _mockTokenService;
    private readonly Mock<DbSet<User>> _mockUserSet;
    private readonly Mock<DbSet<RefreshToken>> _mockRefreshTokenSet;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockTokenService = new Mock<IJwtTokenService>();
        _mockUserSet = new Mock<DbSet<User>>();
        _mockRefreshTokenSet = new Mock<DbSet<RefreshToken>>();

        _mockContext.Setup(c => c.Users).Returns(_mockUserSet.Object);
        _mockContext.Setup(c => c.RefreshTokens).Returns(_mockRefreshTokenSet.Object);

        _handler = new RegisterUserCommandHandler(
            _mockContext.Object,
            _mockPasswordHasher.Object,
            _mockTokenService.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateUserSuccessfully()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "Password123"
        };

        var users = new List<User>().AsQueryable();
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);

        _mockPasswordHasher.Setup(p => p.HashPassword(command.Password))
            .Returns("hashed_password");

        _mockTokenService.Setup(t => t.GenerateAccessToken(It.IsAny<Guid>(), command.Email))
            .Returns("access_token");

        _mockTokenService.Setup(t => t.GenerateRefreshToken())
            .Returns("refresh_token");

        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);
        result.AccessToken.Should().Be("access_token");
        result.RefreshToken.Should().Be("refresh_token");

        _mockUserSet.Verify(u => u.Add(It.IsAny<User>()), Times.Once);
        _mockRefreshTokenSet.Verify(rt => rt.Add(It.IsAny<RefreshToken>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "existing@example.com",
            Password = "Password123"
        };

        var existingUser = new User { Email = command.Email };
        var users = new List<User> { existingUser }.AsQueryable();
        
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        _mockUserSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }
}
