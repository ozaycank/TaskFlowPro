using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Sprints.Commands.CreateSprint;

public class CreateSprintCommandHandler : IRequestHandler<CreateSprintCommand, Guid>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSprintCommandHandler(ISprintRepository sprintRepository, IProjectRepository projectRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _projectRepository = projectRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateSprintCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), request.ProjectId);

        await _authService.AuthorizeRoleAsync(project.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        var sprint = Sprint.Create(request.ProjectId, request.Name, request.Goal, request.StartDate, request.EndDate);

        _sprintRepository.Add(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return sprint.Id;
    }
}