using MediatR;
using TaskFlowPro.Application.Common.Interfaces.Repositories;
using TaskFlowPro.Application.Common.Interfaces.Services;

namespace TaskFlowPro.Application.Workspaces.Queries.GetWorkspaces;

public class GetWorkspacesQueryHandler : IRequestHandler<GetWorkspacesQuery, IEnumerable<WorkspaceDto>>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetWorkspacesQueryHandler(
        IWorkspaceRepository workspaceRepository,
        ICurrentUserService currentUserService)
    {
        _workspaceRepository = workspaceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<WorkspaceDto>> Handle(GetWorkspacesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("User context is missing or invalid.");
        }

        // Fetch workspaces where the user is a member (We implemented this in Phase 4)
        var workspaces = await _workspaceRepository.GetUserWorkspacesAsync(userId, cancellationToken);

        // Project Domain Entities to lightweight Application DTOs
        return workspaces.Select(w => new WorkspaceDto(
            w.Id,
            w.Name,
            w.Description,
            w.OwnerId,
            w.CreatedAt
        ));
    }
}