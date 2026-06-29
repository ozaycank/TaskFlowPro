using MediatR;

namespace Velyo.Application.Workspaces.Commands.DeleteWorkspace;

public record DeleteWorkspaceCommand(Guid WorkspaceId) : IRequest;