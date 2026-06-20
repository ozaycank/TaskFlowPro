using Velyo.Domain.Common.Models;
using Velyo.Domain.Events;

namespace Velyo.Domain.Entities;

public class Attachment : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid? CommentId { get; private set; } // Opsiyonel: Yorumlara da eklenebilir
    public string FileName { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public long FileSizeBytes { get; private set; }
    
    // AWS S3 / MinIO Object Key (Örn: workspaces/{workspaceId}/tasks/{taskId}/{uuid}-{filename})
    public string StorageKey { get; private set; } = null!;
    public bool IsUploaded { get; private set; }

    protected Attachment() { }

    private Attachment(Guid workspaceId, Guid taskId, Guid? commentId, string fileName, string contentType, long fileSizeBytes, string storageKey)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        TaskId = taskId;
        CommentId = commentId;
        FileName = fileName;
        ContentType = contentType;
        FileSizeBytes = fileSizeBytes;
        StorageKey = storageKey;
        IsUploaded = false; // Presigned URL verilince false, yükleme bitince true olur.
    }

    public static Attachment CreatePending(Guid workspaceId, Guid taskId, Guid? commentId, string fileName, string contentType, long fileSizeBytes)
    {
        // Güvenli ve izole edilmiş bulut depolama anahtarı oluşturma
        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var storageKey = $"workspaces/{workspaceId}/tasks/{taskId}/{uniqueFileName}";

        return new Attachment(workspaceId, taskId, commentId, fileName, contentType, fileSizeBytes, storageKey);
    }

    public void MarkAsUploaded()
    {
        if (IsUploaded) throw new InvalidOperationException("Attachment is already marked as uploaded.");
        IsUploaded = true;
        
        // SignalR ve Outbox için Domain Event tetikle
        AddDomainEvent(new AttachmentUploadedEvent(this));
    }
}