using MediatR;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Events;

namespace Velyo.Application.Workspaces.EventHandlers;

public class SendInvitationEmailHandler : INotificationHandler<WorkspaceInvitationCreatedEvent>
{
    private readonly IEmailService _emailService;

    public SendInvitationEmailHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(WorkspaceInvitationCreatedEvent notification, CancellationToken cancellationToken)
    {
        // This is now executing safely in the background Outbox Processor
        await _emailService.SendInvitationEmailAsync(
            notification.Invitation.Email,
            notification.WorkspaceName,
            notification.Invitation.Token,
            cancellationToken);
    }
}