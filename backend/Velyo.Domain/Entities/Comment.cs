using Velyo.Domain.Common.Models;
using Velyo.Domain.Events;

namespace Velyo.Domain.Entities;

public class Comment : AuditableEntity
{
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; } // Author
    public string Content { get; private set; } = null!;

    // Future-proofing for edited comments
    public bool IsEdited { get; private set; }

    protected Comment() { }

    private Comment(Guid taskId, Guid userId, string content)
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        UserId = userId;
        Content = content;
        IsEdited = false;
    }

    public static Comment Create(Guid taskId, Guid userId, string content, Guid workspaceId)
    {
        var comment = new Comment(taskId, userId, content);

        // Trigger Domain Event for Outbox (Notification & SignalR)
        comment.AddDomainEvent(new CommentCreatedEvent(comment, workspaceId));
        return comment;
    }

    public void UpdateContent(string newContent)
    {
        Content = newContent;
        IsEdited = true;
    }
}