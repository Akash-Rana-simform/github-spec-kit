using TaskStatus = TaskManager.Domain.Enums.TaskStatus;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public TaskPriority Priority { get; init; } = TaskPriority.Medium;
}

public record CreateTaskResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public DateTime CreatedAt { get; init; }
}
