using MediatR;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workspaces.Commands.InviteMember;

public record InviteMemberCommand(Guid WorkspaceId, string Email, WorkspaceRole Role) : IRequest<string>;