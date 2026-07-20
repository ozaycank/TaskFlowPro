namespace Velyo.Application.Workspaces.Queries.GetWorkspaceMembers;

public record WorkspaceMemberDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    DateTimeOffset JoinedAt
);