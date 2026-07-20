using MediatR;

namespace Velyo.Application.Workspaces.Queries.GetWorkspaceMembers;

public record GetWorkspaceMembersQuery(Guid WorkspaceId) : IRequest<List<WorkspaceMemberDto>>;