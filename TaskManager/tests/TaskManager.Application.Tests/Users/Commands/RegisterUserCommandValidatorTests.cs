using FluentAssertions;
using FluentValidation.TestHelper;
using TaskManager.Application.Users.Commands.Register;
using Xunit;

namespace TaskManager.Application.Tests.Users.Commands;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator;

    public RegisterUserCommandValidatorTests()
    {
        _validator = new RegisterUserCommandValidator();
    }

    [Fact]
    public void Validate_ValidCommand_ShouldNotHaveErrors()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyEmail_ShouldHaveError(string email)
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = email,
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("test@")]
    [InlineData("@example.com")]
    public void Validate_InvalidEmailFormat_ShouldHaveError(string email)
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = email,
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("short")]
    public void Validate_InvalidPassword_ShouldHaveError(string password)
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Theory]
    [InlineData("password123")] // No uppercase
    [InlineData("PASSWORD123")] // No lowercase
    [InlineData("PasswordABC")] // No digit
    public void Validate_WeakPassword_ShouldHaveError(string password)
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Email = "test@example.com",
            Password = password
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Password);
    }
}
