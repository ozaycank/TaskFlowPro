using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;
using Velyo.Domain.Events;

namespace Velyo.Application.Notifications.EventHandlers;

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
        // 1. Workspace ID'sinin boş (Empty) olmadığından emin ol (Guvenlik katmanı)
        if (notification.Workspace.Id == Guid.Empty)
            return;

        // 2. ActionUrl formatının doğru olduğundan emin ol
        string actionUrl = $"/workspaces/{notification.Workspace.Id}";

        var userNotification = Notification.Create(
            notification.Workspace.Id,
            notification.InitiatedByUserId, // Hedef kullanıcı (Owner)
            NotificationType.System,
            "Workspace Created Successfully",
            $"Welcome to your new workspace: {notification.Workspace.Name}.",
            actionUrl
        );

        _notificationRepository.Add(userNotification);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}