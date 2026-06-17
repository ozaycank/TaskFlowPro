using MediatR;
using Microsoft.Extensions.Logging; // ILogger için eklendi
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;
using Velyo.Domain.Events; // Event artık Domain katmanında

namespace Velyo.Application.Workspaces.Events;

public class WorkspaceCreatedEventHandler : INotificationHandler<WorkspaceCreatedEvent>
{
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkspaceCreatedEventHandler> _logger;

    public WorkspaceCreatedEventHandler(
        IActivityLogRepository activityLogRepository,
        IUnitOfWork unitOfWork,
        ILogger<WorkspaceCreatedEventHandler> logger)
    {
        _activityLogRepository = activityLogRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(WorkspaceCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Velyo Audit Trail: Workspace {WorkspaceId} created by user {UserId}",
            notification.Workspace.Id, notification.InitiatedByUserId);

        var log = ActivityLog.Create(
            notification.Workspace.Id,
            notification.InitiatedByUserId,
            action: "Workspace.Created",
            details: $"Workspace '{notification.Workspace.Name}' was successfully initialized."
        );

        _activityLogRepository.Add(log);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}