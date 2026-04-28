using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void User_ShouldCreateWithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        user.Id.Should().Be(Guid.Empty);
        user.Email.Should().BeEmpty();
        user.PasswordHash.Should().BeEmpty();
        user.IsActive.Should().BeFalse();
        user.Tasks.Should().BeEmpty();
        user.RefreshTokens.Should().BeEmpty();
    }

    [Fact]
    public void User_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var passwordHash = "hashed_password";
        var createdAt = DateTime.UtcNow;

        // Act
        var user = new User
        {
            Id = userId,
            Email = email,
            PasswordHash = passwordHash,
            CreatedAt = createdAt,
            IsActive = true
        };

        // Assert
        user.Id.Should().Be(userId);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.CreatedAt.Should().Be(createdAt);
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void User_ShouldHaveNavigationProperties()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var task = new TaskItem { Id = Guid.NewGuid(), UserId = user.Id };
        var refreshToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id };

        // Act
        user.Tasks.Add(task);
        user.RefreshTokens.Add(refreshToken);

        // Assert
        user.Tasks.Should().HaveCount(1);
        user.Tasks.First().Should().Be(task);
        user.RefreshTokens.Should().HaveCount(1);
        user.RefreshTokens.First().Should().Be(refreshToken);
    }
}
