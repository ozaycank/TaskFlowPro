using MediatR;
using Microsoft.Extensions.Logging;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;
using Velyo.Domain.Events;

namespace Velyo.Application.Workspaces.Events;

public class WorkspaceCreatedEventHandler : INotificationHandler<WorkspaceCreatedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IWorkflowRepository _workflowRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkspaceCreatedEventHandler> _logger;

    public WorkspaceCreatedEventHandler(
        IActivityLogRepository activityLogRepository,
        IWorkflowRepository workflowRepository,
        IUnitOfWork unitOfWork,
        ILogger<WorkspaceCreatedEventHandler> logger)
    {
        _activityLogRepository = activityLogRepository;
        _workflowRepository = workflowRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(WorkspaceCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Velyo Audit Trail: Workspace {WorkspaceId} created by user {UserId}",
            notification.Workspace.Id, notification.InitiatedByUserId);

        // DÜZELTİLDİ: Yeni ActivityLog parametrelerine uygun hale getirildi.
        var log = ActivityLog.Create(
            workspaceId: notification.Workspace.Id,
            projectId: null,
            taskId: null,
            userId: notification.InitiatedByUserId,
            entityType: "Workspace",
            entityId: notification.Workspace.Id,
            action: "Workspace.Created",
            details: $"Workspace '{notification.Workspace.Name}' was successfully initialized."
        );

        _activityLogRepository.Add(log);

        // FIXED: Seed a default Workflow so the Kanban Board is immediately usable
        var workflow = Workflow.Create(notification.Workspace.Id, "Default Workflow", true);
        workflow.AddState("To Do", "#e2e8f0", StateCategory.Unstarted, 100);
        workflow.AddState("In Progress", "#60a5fa", StateCategory.Started, 200);
        workflow.AddState("In Review", "#fde047", StateCategory.Started, 300);
        workflow.AddState("Done", "#4ade80", StateCategory.Completed, 400);

        _workflowRepository.Add(workflow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}