using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workspaces.Commands.CreateWorkspace;

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