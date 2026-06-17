using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Api.Services;

public class DevelopmentCurrentUserService : ICurrentUserService
{
    // A fixed, valid Guid for testing purposes
    public string? UserId => "00000000-0000-0000-0000-000000000001";
    public string? Email => "testadmin@velyo.local";
    public bool IsAuthenticated => true;
}