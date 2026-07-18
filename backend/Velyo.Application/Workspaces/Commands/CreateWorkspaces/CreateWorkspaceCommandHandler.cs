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
    private readonly IWorkflowRepository _workflowRepository; // FIX: Mimarinize uygun Repository eklendi
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkspaceCommandHandler(
        IWorkspaceRepository workspaceRepository,
        IWorkspaceMemberRepository workspaceMemberRepository,
        IWorkflowRepository workflowRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _workspaceRepository = workspaceRepository;
        _workspaceMemberRepository = workspaceMemberRepository;
        _workflowRepository = workflowRepository;
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
        _workspaceMemberRepository.Add(workspaceMember);

        // --- FIX: CS1503 hatasını çözmek için 3. parametreye doğru Enum tipini verdik ---
        var defaultWorkflow = Workflow.Create(workspace.Id, "Default Workflow", isDefault: true);

        // AddState(İsim, Renk/Açıklama, Kategori, Sıra) formatına uygun hale getirildi.
        defaultWorkflow.AddState("To Do", "#e2e8f0", StateCategory.ToDo, 0);
        defaultWorkflow.AddState("In Progress", "#bfdbfe", StateCategory.InProgress, 1);
        defaultWorkflow.AddState("Done", "#bbf7d0", StateCategory.Done, 2);

        _workflowRepository.Add(defaultWorkflow);
        // ----------------------------------------------------------------------------------

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return workspace.Id;
    }
}