using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskStatus = TaskManager.Domain.Enums.TaskStatus;
using Xunit;

namespace TaskManager.Domain.Tests.Entities;

public class TaskItemTests
{
    [Fact]
    public void TaskItem_ShouldCreateWithDefaultValues()
    {
        // Arrange & Act
        var task = new TaskItem();

        // Assert
        task.Id.Should().Be(Guid.Empty);
        task.UserId.Should().Be(Guid.Empty);
        task.Title.Should().BeEmpty();
        task.Description.Should().BeNull();
        task.Status.Should().Be(TaskStatus.ToDo);
        task.Priority.Should().Be(TaskPriority.Medium);
        task.RowVersion.Should().BeEmpty();
    }

    [Fact]
    public void TaskItem_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var title = "Test Task";
        var description = "Test Description";
        var createdAt = DateTime.UtcNow;
        var modifiedAt = DateTime.UtcNow;

        // Act
        var task = new TaskItem
        {
            Id = taskId,
            UserId = userId,
            Title = title,
            Description = description,
            Status = TaskStatus.InProgress,
            Priority = TaskPriority.High,
            CreatedAt = createdAt,
            ModifiedAt = modifiedAt
        };

        // Assert
        task.Id.Should().Be(taskId);
        task.UserId.Should().Be(userId);
        task.Title.Should().Be(title);
        task.Description.Should().Be(description);
        task.Status.Should().Be(TaskStatus.InProgress);
        task.Priority.Should().Be(TaskPriority.High);
        task.CreatedAt.Should().Be(createdAt);
        task.ModifiedAt.Should().Be(modifiedAt);
    }

    [Fact]
    public void TaskItem_ShouldAllowNullDescription()
    {
        // Arrange & Act
        var task = new TaskItem
        {
            Title = "Task without description",
            Description = null
        };

        // Assert
        task.Description.Should().BeNull();
    }

    [Theory]
    [InlineData(TaskStatus.ToDo)]
    [InlineData(TaskStatus.InProgress)]
    [InlineData(TaskStatus.Done)]
    public void TaskItem_ShouldAcceptAllValidStatuses(TaskStatus status)
    {
        // Arrange & Act
        var task = new TaskItem { Status = status };

        // Assert
        task.Status.Should().Be(status);
    }

    [Theory]
    [InlineData(TaskPriority.Low)]
    [InlineData(TaskPriority.Medium)]
    [InlineData(TaskPriority.High)]
    [InlineData(TaskPriority.Critical)]
    public void TaskItem_ShouldAcceptAllValidPriorities(TaskPriority priority)
    {
        // Arrange & Act
        var task = new TaskItem { Priority = priority };

        // Assert
        task.Priority.Should().Be(priority);
    }
}
