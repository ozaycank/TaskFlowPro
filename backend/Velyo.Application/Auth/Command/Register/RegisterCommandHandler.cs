using MediatR;
using Velyo.Application.Auth.Commands.Login;
using Velyo.Application.Auth.Services;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Application.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.GetByEmailAsync(request.Email, cancellationToken) != null)
            throw new ValidationException(new[] { new FluentValidation.Results.ValidationFailure("Email", "Email is already in use.") });

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = User.Create(request.Email, request.FirstName, request.LastName, passwordHash);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Refresh token valid for 7 days
        user.UpdateRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponseDto(user.Id, user.Email, user.FirstName, user.LastName, accessToken, refreshToken);
    }
}