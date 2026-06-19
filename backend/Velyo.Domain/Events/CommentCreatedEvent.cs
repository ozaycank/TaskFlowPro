using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class CommentCreatedEvent : DomainEvent
{
    public Comment Comment { get; }
    public Guid WorkspaceId { get; }

    public CommentCreatedEvent(Comment comment, Guid workspaceId)
    {
        Comment = comment;
        WorkspaceId = workspaceId;
    }
}