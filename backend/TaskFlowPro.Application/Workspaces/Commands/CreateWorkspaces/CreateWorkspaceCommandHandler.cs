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
    private readonly IWorkspaceMemberRepository _workspaceMemberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkspaceCommandHandler(
        IWorkspaceRepository workspaceRepository,
        IWorkspaceMemberRepository workspaceMemberRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _workspaceRepository = workspaceRepository;
        _workspaceMemberRepository = workspaceMemberRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var ownerId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or UserId is invalid.");
        }

        var workspace = Workspace.Create(request.Name, request.Description, ownerId);
        var workspaceMember = WorkspaceMember.Create(workspace.Id, ownerId, WorkspaceRole.Owner);

        _workspaceRepository.Add(workspace);
        _workspaceMemberRepository.Add(workspaceMember); // Fixed missing persistence

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return workspace.Id;
    }
}