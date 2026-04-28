using TaskStatus = TaskManager.Domain.Enums.TaskStatus;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public TaskStatus? Status { get; init; }
    public TaskPriority? Priority { get; init; }
}

public record UpdateTaskResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public DateTime ModifiedAt { get; init; }
}
