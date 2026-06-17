namespace Velyo.Application.Auth.Commands.Login;

public record AuthResponseDto(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string AccessToken,
    string RefreshToken);