using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Sprints.Commands.UpdateSprint;

public class UpdateSprintCommandHandler : IRequestHandler<UpdateSprintCommand>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSprintCommandHandler(ISprintRepository sprintRepository, IProjectRepository projectRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _projectRepository = projectRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateSprintCommand request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint == null) throw new NotFoundException(nameof(Sprint), request.SprintId);

        var project = await _projectRepository.GetByIdAsync(sprint.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), sprint.ProjectId);

        await _authService.AuthorizeRoleAsync(project.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        // Assuming you add an UpdateDetails method to Sprint.cs. If not, map properties here.
        var propertyInfo = typeof(Sprint).GetProperty("Name");
        if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(sprint, request.Name);

        propertyInfo = typeof(Sprint).GetProperty("Goal");
        if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(sprint, request.Goal);

        propertyInfo = typeof(Sprint).GetProperty("StartDate");
        if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(sprint, request.StartDate);

        propertyInfo = typeof(Sprint).GetProperty("EndDate");
        if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(sprint, request.EndDate);

        _sprintRepository.Update(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}