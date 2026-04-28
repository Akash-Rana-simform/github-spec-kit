namespace TaskManager.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email);
    string GenerateRefreshToken();
    Guid? ValidateAccessToken(string token);
}
