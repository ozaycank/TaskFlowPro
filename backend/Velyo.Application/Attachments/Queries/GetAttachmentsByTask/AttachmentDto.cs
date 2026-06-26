namespace Velyo.Application.Attachments.Queries.GetAttachmentsByTask;

public record AttachmentDto(
    Guid Id,
    Guid TaskId,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    bool IsUploaded,
    DateTimeOffset CreatedAt);