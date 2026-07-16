using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectCommandHandler(
        IProjectRepository projectRepository,
        IWorkspaceAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project == null)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId.ToString());
        }

        // Ensure user has access to the workspace this project belongs to
        await _authorizationService.AuthorizeMembershipAsync(project.WorkspaceId, cancellationToken);

        _projectRepository.Delete(project);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}