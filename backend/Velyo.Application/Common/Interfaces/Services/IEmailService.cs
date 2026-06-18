namespace Velyo.Application.Common.Interfaces.Services;

public interface IEmailService
{
    Task SendInvitationEmailAsync(string toEmail, string workspaceName, string token, CancellationToken cancellationToken = default);
}