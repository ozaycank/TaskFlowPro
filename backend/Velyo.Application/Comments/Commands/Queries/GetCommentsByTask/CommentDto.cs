namespace Velyo.Application.Comments.Queries.GetCommentsByTask;

public record CommentDto(
    Guid Id,
    Guid TaskId,
    Guid UserId,
    string Content,
    bool IsEdited,
    DateTimeOffset CreatedAt,
    string? CreatedBy);