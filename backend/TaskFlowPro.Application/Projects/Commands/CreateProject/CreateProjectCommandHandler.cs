using MediatR;
using TaskFlowPro.Application.Common.Interfaces.Data;
using TaskFlowPro.Application.Common.Interfaces.Repositories;
using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IWorkspaceRepository workspaceRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _workspaceRepository = workspaceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        // Business Rule: Ensure the workspace exists
        var workspace = await _workspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace == null)
        {
            // In a real SaaS, consider a custom DomainException like NotFoundException
            throw new InvalidOperationException($"Workspace with ID {request.WorkspaceId} not found.");
        }

        // Create domain entity
        var project = Project.Create(request.WorkspaceId, request.Name, request.Description);

        // Persist
        _projectRepository.Add(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}