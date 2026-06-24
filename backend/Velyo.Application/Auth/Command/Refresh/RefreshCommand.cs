using MediatR;
using Velyo.Application.Auth.Commands.Login;

namespace Velyo.Application.Auth.Commands.Refresh;

public record RefreshCommand(string RefreshToken) : IRequest<AuthResponseDto>;