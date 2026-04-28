using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTaskCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateTaskResponse> Handle(
        UpdateTaskCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == _currentUserService.UserId.Value, 
                cancellationToken);

        if (task == null)
        {
            throw new InvalidOperationException("Task not found or access denied");
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Title))
        {
            task.Title = request.Title;
        }

        if (request.Description != null)
        {
            task.Description = request.Description;
        }

        if (request.Status.HasValue)
        {
            task.Status = request.Status.Value;
        }

        if (request.Priority.HasValue)
        {
            task.Priority = request.Priority.Value;
        }

        task.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateTaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            ModifiedAt = task.ModifiedAt
        };
    }
}
