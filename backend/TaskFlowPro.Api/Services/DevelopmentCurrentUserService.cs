using TaskFlowPro.Application.Common.Interfaces.Services;

namespace TaskFlowPro.Api.Services;

public class DevelopmentCurrentUserService : ICurrentUserService
{
    // A fixed, valid Guid for testing purposes
    public string? UserId => "00000000-0000-0000-0000-000000000001";
    public string? Email => "testadmin@taskflowpro.local";
    public bool IsAuthenticated => true;
}