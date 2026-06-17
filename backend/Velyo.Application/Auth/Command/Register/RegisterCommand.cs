using MediatR;
using Velyo.Application.Auth.Commands.Login;
namespace Velyo.Application.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : IRequest<AuthResponseDto>;