using MediatR;
namespace Velyo.Application.Workspaces.Commands.AcceptInvitation;

public record AcceptInvitationCommand(string Token) : IRequest;