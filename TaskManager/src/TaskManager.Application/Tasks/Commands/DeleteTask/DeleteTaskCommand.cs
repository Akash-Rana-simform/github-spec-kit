namespace TaskManager.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand
{
    public Guid Id { get; init; }
}
