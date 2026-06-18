namespace Velyo.Infrastructure.RealTime.Constants;

public static class HubGroups
{
    public static string WorkspaceGroup(Guid workspaceId) => $"workspace_{workspaceId}";
}