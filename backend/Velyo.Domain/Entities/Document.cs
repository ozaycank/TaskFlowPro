using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class Document : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid? ProjectId { get; private set; }
    public Guid? ParentDocumentId { get; private set; }
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public string? EmojiIcon { get; private set; }
    public float OrderIndex { get; private set; }

    protected Document() { }

    private Document(Guid workspaceId, Guid? projectId, Guid? parentDocumentId, string title, string content, string? emojiIcon, float orderIndex)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        ParentDocumentId = parentDocumentId;
        Title = title;
        Content = content;
        EmojiIcon = emojiIcon;
        OrderIndex = orderIndex;
    }

    public static Document Create(Guid workspaceId, Guid? projectId, Guid? parentDocumentId, string title, string content, string? emojiIcon, float orderIndex)
    {
        return new Document(workspaceId, projectId, parentDocumentId, title, content, emojiIcon, orderIndex);
    }

    public void UpdateContent(string title, string content, string? emojiIcon)
    {
        Title = title;
        Content = content;
        EmojiIcon = emojiIcon;
    }

    public void Move(Guid? newParentDocumentId, float newOrderIndex)
    {
        ParentDocumentId = newParentDocumentId;
        OrderIndex = newOrderIndex;
    }
}