using MediatR;

namespace Velyo.Application.Workspaces.Queries.GetWorkspaces;

// Returns a read-only list of WorkspaceDtos for the authenticated user
public record GetWorkspacesQuery : IRequest<IEnumerable<WorkspaceDto>>;