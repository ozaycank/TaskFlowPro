using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;
using Velyo.Domain.Events;

namespace Velyo.Application.Notifications.EventHandlers;

// Listens to the EXACT SAME event as ActivityLog, but creates a user notification
public class WorkspaceCreatedNotificationHandler : INotificationHandler<WorkspaceCreatedEvent>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WorkspaceCreatedNotificationHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(WorkspaceCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Owner gets a welcome notification
        var userNotification = Notification.Create(
            notification.Workspace.Id,
            notification.InitiatedByUserId, // Target user
            NotificationType.System,
            "Workspace Created Successfully",
            $"Welcome to your new workspace: {notification.Workspace.Name}.",
            $"/workspaces/{notification.Workspace.Id}" // Future React route
        );

        _notificationRepository.Add(userNotification);

        // No need to call SaveChangesAsync here if the UnitOfWork pipeline 
        // is designed to group all event executions into the SAME transaction.
        // If MediatR Publish is called AFTER SaveChanges (as done in Phase 13), 
        // we MUST call SaveChangesAsync here to persist the notification.
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}