using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Application.Workspaces.Queries.GetWorkspaces;
using Velyo.Domain.Entities;

namespace Velyo.Application.Workspaces.Queries.GetWorkspaceById;

public class GetWorkspaceByIdQueryHandler : IRequestHandler<GetWorkspaceByIdQuery, WorkspaceDto>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspaceAuthorizationService _authService;

    public GetWorkspaceByIdQueryHandler(IWorkspaceRepository workspaceRepository, IWorkspaceAuthorizationService authService)
    {
        _workspaceRepository = workspaceRepository;
        _authService = authService;
    }

    public async Task<WorkspaceDto> Handle(GetWorkspaceByIdQuery request, CancellationToken cancellationToken)
    {
        await _authService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace == null) throw new NotFoundException(nameof(Workspace), request.WorkspaceId);

        return new WorkspaceDto(
            workspace.Id,
            workspace.Name,
            workspace.Description,
            workspace.OwnerId,
            workspace.CreatedAt
        );
    }
}