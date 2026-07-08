namespace Velyo.Application.Documents.Queries.GetDocumentById;

public record DocumentDetailDto(
    Guid Id,
    Guid WorkspaceId,
    Guid? ProjectId,
    Guid? ParentDocumentId,
    string Title,
    string Content,
    string? EmojiIcon,
    float OrderIndex,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastModifiedAt);