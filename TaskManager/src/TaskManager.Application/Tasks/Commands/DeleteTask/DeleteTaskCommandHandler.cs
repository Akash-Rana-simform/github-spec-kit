using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;

namespace TaskManager.Application.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTaskCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(
        DeleteTaskCommand request,
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

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
