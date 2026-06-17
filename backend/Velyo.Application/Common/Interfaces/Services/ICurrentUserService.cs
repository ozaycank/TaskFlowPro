namespace Velyo.Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}