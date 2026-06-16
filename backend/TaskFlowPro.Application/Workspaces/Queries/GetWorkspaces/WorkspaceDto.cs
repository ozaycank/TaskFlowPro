namespace TaskFlowPro.Application.Workspaces.Queries.GetWorkspaces;

public record WorkspaceDto(
    Guid Id,
    string Name,
    string? Description,
    Guid OwnerId,
    DateTimeOffset CreatedAt);