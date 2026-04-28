using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.Commands.CreateTask;
using TaskManager.Application.Tasks.Commands.DeleteTask;
using TaskManager.Application.Tasks.Commands.UpdateTask;
using TaskManager.Application.Tasks.Queries.GetTaskById;
using TaskManager.Application.Tasks.Queries.GetTasks;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly CreateTaskCommandHandler _createTaskHandler;
    private readonly UpdateTaskCommandHandler _updateTaskHandler;
    private readonly DeleteTaskCommandHandler _deleteTaskHandler;
    private readonly GetTasksQueryHandler _getTasksHandler;
    private readonly GetTaskByIdQueryHandler _getTaskByIdHandler;
    private readonly CreateTaskCommandValidator _createTaskValidator;
    private readonly UpdateTaskCommandValidator _updateTaskValidator;

    public TasksController(
        CreateTaskCommandHandler createTaskHandler,
        UpdateTaskCommandHandler updateTaskHandler,
        DeleteTaskCommandHandler deleteTaskHandler,
        GetTasksQueryHandler getTasksHandler,
        GetTaskByIdQueryHandler getTaskByIdHandler,
        CreateTaskCommandValidator createTaskValidator,
        UpdateTaskCommandValidator updateTaskValidator)
    {
        _createTaskHandler = createTaskHandler;
        _updateTaskHandler = updateTaskHandler;
        _deleteTaskHandler = deleteTaskHandler;
        _getTasksHandler = getTasksHandler;
        _getTaskByIdHandler = getTaskByIdHandler;
        _createTaskValidator = createTaskValidator;
        _updateTaskValidator = updateTaskValidator;
    }

    [HttpGet]
    public async Task<ActionResult<GetTasksResponse>> GetTasks(
        [FromQuery] TaskStatus? status,
        [FromQuery] TaskPriority? priority,
        [FromQuery] string? searchTerm,
        CancellationToken cancellationToken)
    {
        var query = new GetTasksQuery
        {
            Status = status,
            Priority = priority,
            SearchTerm = searchTerm
        };

        var result = await _getTasksHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetTaskByIdResponse>> GetTaskById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetTaskByIdQuery { Id = id };
            var result = await _getTaskByIdHandler.Handle(query, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<CreateTaskResponse>> CreateTask(
        [FromBody] CreateTaskCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createTaskValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        var result = await _createTaskHandler.Handle(command, cancellationToken);
        return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateTaskResponse>> UpdateTask(
        Guid id,
        [FromBody] UpdateTaskCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(new { message = "ID in URL does not match ID in request body" });
        }

        var validationResult = await _updateTaskValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        try
        {
            var result = await _updateTaskHandler.Handle(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteTaskCommand { Id = id };
            await _deleteTaskHandler.Handle(command, cancellationToken);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
