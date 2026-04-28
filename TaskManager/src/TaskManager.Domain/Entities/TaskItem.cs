using TaskStatus = TaskManager.Domain.Enums.TaskStatus;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

/// <summary>
/// Represents a work item owned by a user
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    
    // Navigation properties
    public User User { get; set; } = null!;
}
