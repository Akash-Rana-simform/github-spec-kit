using FluentAssertions;
using TaskManager.Domain.Entities;
using Xunit;

namespace TaskManager.Domain.Tests.Entities;

public class RefreshTokenTests
{
    [Fact]
    public void RefreshToken_ShouldCreateWithDefaultValues()
    {
        // Arrange & Act
        var refreshToken = new RefreshToken();

        // Assert
        refreshToken.Id.Should().Be(Guid.Empty);
        refreshToken.UserId.Should().Be(Guid.Empty);
        refreshToken.Token.Should().BeEmpty();
        refreshToken.IsRevoked.Should().BeFalse();
    }

    [Fact]
    public void RefreshToken_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var tokenId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var token = "refresh_token_string";
        var expiresAt = DateTime.UtcNow.AddDays(7);
        var createdAt = DateTime.UtcNow;

        // Act
        var refreshToken = new RefreshToken
        {
            Id = tokenId,
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt,
            CreatedAt = createdAt,
            IsRevoked = false
        };

        // Assert
        refreshToken.Id.Should().Be(tokenId);
        refreshToken.UserId.Should().Be(userId);
        refreshToken.Token.Should().Be(token);
        refreshToken.ExpiresAt.Should().Be(expiresAt);
        refreshToken.CreatedAt.Should().Be(createdAt);
        refreshToken.IsRevoked.Should().BeFalse();
    }

    [Fact]
    public void RefreshToken_ShouldAllowRevocation()
    {
        // Arrange
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = "test_token",
            IsRevoked = false
        };

        // Act
        refreshToken.IsRevoked = true;

        // Assert
        refreshToken.IsRevoked.Should().BeTrue();
    }

    [Fact]
    public void RefreshToken_ShouldHaveUserNavigation()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user
        };

        // Assert
        refreshToken.User.Should().Be(user);
        refreshToken.User.Id.Should().Be(user.Id);
    }
}
