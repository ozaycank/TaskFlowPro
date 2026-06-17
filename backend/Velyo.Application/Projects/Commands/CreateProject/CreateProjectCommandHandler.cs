using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkspaceAuthorizationService _authorizationService;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IWorkspaceRepository workspaceRepository,
        IUnitOfWork unitOfWork,
        IWorkspaceAuthorizationService authorizationService)
    {
        _projectRepository = projectRepository;
        _workspaceRepository = workspaceRepository;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // 1. TENANT ISOLATION CHECK: Fail fast if the user is not a member of this workspace
        await _authorizationService.AuthorizeMembershipAsync(request.WorkspaceId, cancellationToken);

        // Business Rule: Ensure the workspace exists
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace == null)
        {
            throw new NotFoundException(nameof(Workspace), request.WorkspaceId);
        }

        // Create domain entity
        var project = Project.Create(request.WorkspaceId, request.Name, request.Description);

        // Persist
        _projectRepository.Add(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}