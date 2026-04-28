using TaskStatus = TaskManager.Domain.Enums.TaskStatus;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public record GetTasksQuery
{
    public TaskStatus? Status { get; init; }
    public TaskPriority? Priority { get; init; }
    public string? SearchTerm { get; init; }
}

public record TaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public TaskStatus Status { get; init; }
    public TaskPriority Priority { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ModifiedAt { get; init; }
}

public record GetTasksResponse
{
    public List<TaskDto> Tasks { get; init; } = new();
}
