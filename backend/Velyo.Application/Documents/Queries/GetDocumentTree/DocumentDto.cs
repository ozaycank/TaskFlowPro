namespace Velyo.Application.Documents.Queries.GetDocumentTree;

public record DocumentDto(
    Guid Id,
    Guid WorkspaceId,
    Guid? ProjectId,
    Guid? ParentDocumentId,
    string Title,
    string? EmojiIcon,
    float OrderIndex,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastModifiedAt);