using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Sprints.Commands.StartSprint;

public class StartSprintCommandHandler : IRequestHandler<StartSprintCommand>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public StartSprintCommandHandler(ISprintRepository sprintRepository, IProjectRepository projectRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _projectRepository = projectRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(StartSprintCommand request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint == null) throw new NotFoundException(nameof(Sprint), request.SprintId);

        var project = await _projectRepository.GetByIdAsync(sprint.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), sprint.ProjectId);

        await _authService.AuthorizeRoleAsync(project.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        sprint.Start();

        _sprintRepository.Update(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}