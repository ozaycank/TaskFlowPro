using Velyo.Domain.Entities;

namespace Velyo.Application.Auth.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}