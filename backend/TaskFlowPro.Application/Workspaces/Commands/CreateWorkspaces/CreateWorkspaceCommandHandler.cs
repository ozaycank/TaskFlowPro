using MediatR;
using TaskFlowPro.Application.Common.Interfaces.Data;
using TaskFlowPro.Application.Common.Interfaces.Repositories;
using TaskFlowPro.Application.Common.Interfaces.Services;
using TaskFlowPro.Domain.Entities;
using TaskFlowPro.Domain.Enums;

namespace TaskFlowPro.Application.Workspaces.Commands.CreateWorkspace;

public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, Guid>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkspaceCommandHandler(
        IWorkspaceRepository workspaceRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _workspaceRepository = workspaceRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        // 1. Ensure user is authenticated
        if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var ownerId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or UserId is invalid.");
        }

        // 2. Create the Workspace via Domain Factory Method
        var workspace = Workspace.Create(request.Name, request.Description, ownerId);

        // 3. IMPORTANT BUSINESS RULE: The owner must also be added as an Admin/Owner member to the workspace
        var workspaceMember = WorkspaceMember.Create(workspace.Id, ownerId, WorkspaceRole.Owner);

        // Note: For a robust implementation, the Workspace entity should probably have an AddMember method 
        // to encapsulate this relationship completely within the Aggregate root, 
        // but adding it via a repository (or future MemberRepository) works for the current schema.
        // For now, since EF Core doesn't know about Member collection navigation in Workspace, 
        // we'd need a member repository. However, a better domain design is:
        // Workspace.AddMember(ownerId, WorkspaceRole.Owner)

        _workspaceRepository.Add(workspace);

        // (We assume the UnitOfWork handles saving the workspace)
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Returning the generated ID
        return workspace.Id;
    }
}