using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Application.Notifications.Queries.GetMyNotifications;

public record GetMyNotificationsQuery(bool UnreadOnly = false) : IRequest<IEnumerable<NotificationDto>>;

public class GetMyNotificationsQueryHandler : IRequestHandler<GetMyNotificationsQuery, IEnumerable<NotificationDto>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyNotificationsQueryHandler(INotificationRepository notificationRepository, ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(GetMyNotificationsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) || !Guid.TryParse(_currentUserService.UserId, out var userId))
            throw new UnauthorizedAccessException();

        var notifications = request.UnreadOnly
            ? await _notificationRepository.GetUnreadByUserIdAsync(userId, cancellationToken)
            : await _notificationRepository.GetAllByUserIdAsync(userId, 50, cancellationToken);

        return notifications.Select(n => new NotificationDto(
            n.Id, n.WorkspaceId, n.Type, n.Title, n.Message, n.ActionUrl, n.IsRead, n.CreatedAt));
    }
}