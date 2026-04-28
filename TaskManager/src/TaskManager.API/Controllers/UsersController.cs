using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Users.Commands.Login;
using TaskManager.Application.Users.Commands.Register;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly RegisterUserCommandHandler _registerHandler;
    private readonly LoginCommandHandler _loginHandler;
    private readonly RegisterUserCommandValidator _registerValidator;
    private readonly LoginCommandValidator _loginValidator;

    public UsersController(
        RegisterUserCommandHandler registerHandler,
        LoginCommandHandler loginHandler,
        RegisterUserCommandValidator registerValidator,
        LoginCommandValidator loginValidator)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResponse>> Register(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _registerValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        try
        {
            var result = await _registerHandler.Handle(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _loginValidator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
        }

        try
        {
            var result = await _loginHandler.Handle(command, cancellationToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
