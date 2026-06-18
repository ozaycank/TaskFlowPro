using Microsoft.Extensions.Logging;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Infrastructure.Services;

public class MockEmailService : IEmailService
{
    private readonly ILogger<MockEmailService> _logger;

    public MockEmailService(ILogger<MockEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendInvitationEmailAsync(string toEmail, string workspaceName, string token, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("========== MOCK EMAIL SENT ==========");
        _logger.LogWarning("To      : {Email}", toEmail);
        _logger.LogWarning("Subject : You have been invited to {WorkspaceName}", workspaceName);
        _logger.LogWarning("Body    : Click here to join: http://localhost:3000/invite/accept?token={Token}", token);
        _logger.LogWarning("=====================================");

        return Task.CompletedTask;
    }
}