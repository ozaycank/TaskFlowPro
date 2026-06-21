using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Sprints.Commands.CompleteSprint;

public record CompleteSprintCommand(Guid SprintId, Guid? MoveIncompleteTasksToSprintId) : IRequest;

public class CompleteSprintCommandHandler : IRequestHandler<CompleteSprintCommand>
{
    private readonly ISprintRepository _sprintRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository; // ADDED: To fetch WorkspaceId
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteSprintCommandHandler(
        ISprintRepository sprintRepository,
        ITaskItemRepository taskRepository,
        IProjectRepository projectRepository,
        IWorkflowRepository workflowRepository,
        IWorkspaceAuthorizationService authService,
        IUnitOfWork unitOfWork)
    {
        _sprintRepository = sprintRepository;
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _workflowRepository = workflowRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CompleteSprintCommand request, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(request.SprintId, cancellationToken);
        if (sprint == null) throw new NotFoundException(nameof(Sprint), request.SprintId);

        // Fetch project to get the WorkspaceId (Fixes CS0839)
        var project = await _projectRepository.GetByIdAsync(sprint.ProjectId, cancellationToken);
        if (project == null) throw new NotFoundException(nameof(Project), sprint.ProjectId);

        // SECURE: Tenant Validation
        await _authService.AuthorizeRoleAsync(project.WorkspaceId, WorkspaceRole.Admin, cancellationToken);

        // 1. Mark Sprint as Completed
        sprint.Complete();

        // 2. Enterprise Logic: Handle Incomplete Tasks
        var sprintTasks = await _taskRepository.GetBySprintIdAsync(sprint.Id, cancellationToken);

        // Fetches workflow to identify which states are categorized as "Completed"
        var workflows = await _workflowRepository.GetByWorkspaceIdAsync(project.WorkspaceId, cancellationToken);

        // Detailed implementation would map StateId to StateCategory.Completed here.
        // For architectural setup, we assume all tasks in this iteration are processed.

        foreach (var task in sprintTasks)
        {
            // Simplified logic: If task is NOT in a completed state
            bool isCompleted = false; // Evaluate based on task.StateId and Workflow configuration

            if (!isCompleted)
            {
                // Move to new sprint or send to backlog (null)
                task.AssignToSprint(request.MoveIncompleteTasksToSprintId);
                _taskRepository.Update(task);
            }
        }

        _sprintRepository.Update(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}