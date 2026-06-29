using MediatR;
using Velyo.Application.Workspaces.Queries.GetWorkspaces;

namespace Velyo.Application.Workspaces.Queries.GetWorkspaceById;

public record GetWorkspaceByIdQuery(Guid WorkspaceId) : IRequest<WorkspaceDto>;