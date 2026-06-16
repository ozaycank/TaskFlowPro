namespace TaskFlowPro.Application.Projects.Queries.GetProjects;

public record ProjectDto(
    Guid Id,
    Guid WorkspaceId,
    string Name,
    string? Description,
    DateTimeOffset CreatedAt);