using FluentAssertions;
using Moq;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Application.Tasks.Commands.CreateTask;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Application.Tests.Tasks.Commands;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<IApplicationDbContext> _mockContext;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<DbSet<TaskItem>> _mockTaskSet;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockTaskSet = new Mock<DbSet<TaskItem>>();

        _mockContext.Setup(c => c.Tasks).Returns(_mockTaskSet.Object);

        _handler = new CreateTaskCommandHandler(
            _mockContext.Object,
            _mockCurrentUserService.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTaskSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTaskCommand
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = TaskPriority.High
        };

        _mockCurrentUserService.Setup(u => u.IsAuthenticated).Returns(true);
        _mockCurrentUserService.Setup(u => u.UserId).Returns(userId);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(command.Title);
        result.Description.Should().Be(command.Description);
        result.Priority.Should().Be(TaskPriority.High);
        result.Status.Should().Be(TaskStatus.ToDo);

        _mockTaskSet.Verify(t => t.Add(It.IsAny<TaskItem>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UnauthenticatedUser_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            Title = "Test Task"
        };

        _mockCurrentUserService.Setup(u => u.IsAuthenticated).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_TaskWithoutDescription_ShouldCreateSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTaskCommand
        {
            Title = "Task without description",
            Description = null,
            Priority = TaskPriority.Medium
        };

        _mockCurrentUserService.Setup(u => u.IsAuthenticated).Returns(true);
        _mockCurrentUserService.Setup(u => u.UserId).Returns(userId);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Description.Should().BeNull();
        _mockTaskSet.Verify(t => t.Add(It.IsAny<TaskItem>()), Times.Once);
    }
}
