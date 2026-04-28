namespace TaskManager.Application.Users.Commands.Register;

public record RegisterUserCommand
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record RegisterUserResponse
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}
