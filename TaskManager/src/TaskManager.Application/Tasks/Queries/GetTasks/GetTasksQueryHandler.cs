using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public class GetTasksQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTasksQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<GetTasksResponse> Handle(
        GetTasksQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated || !_currentUserService.UserId.HasValue)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var query = _context.Tasks
            .Where(t => t.UserId == _currentUserService.UserId.Value);

        // Apply filters
        if (request.Status.HasValue)
        {
            query = query.Where(t => t.Status == request.Status.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(t => t.Priority == request.Priority.Value);
        }

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(t => 
                t.Title.Contains(request.SearchTerm) || 
                (t.Description != null && t.Description.Contains(request.SearchTerm)));
        }

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                CreatedAt = t.CreatedAt,
                ModifiedAt = t.ModifiedAt
            })
            .ToListAsync(cancellationToken);

        return new GetTasksResponse
        {
            Tasks = tasks
        };
    }
}
