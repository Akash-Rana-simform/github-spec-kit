using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Application.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTaskByIdQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTaskByIdResponse> Handle(
        GetTaskByIdQuery request,
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

        return new GetTaskByIdResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt,
            ModifiedAt = task.ModifiedAt
        };
    }
}
