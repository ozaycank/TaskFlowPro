using MediatR;
using Velyo.Application.Auth.Commands.Login;
using Velyo.Application.Auth.Services;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;

namespace Velyo.Application.Auth.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public RefreshCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        // 1. Locate the user by the provided refresh token
        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        // 2. Validate token existence and expiry
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        // 3. Generate new tokens
        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // 4. Update the user entity with the new refresh token (Rolling Refresh Tokens)
        user.UpdateRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Return the new payload matching the frontend's expected contract
        return new AuthResponseDto(user.Id, user.Email, user.FirstName, user.LastName, newAccessToken, newRefreshToken);
    }
}