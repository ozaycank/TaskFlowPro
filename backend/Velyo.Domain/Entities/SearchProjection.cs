using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

// Bu Entity, sadece CQRS "Query" tarafında arama yapmak için kullanılacak olan, 
// Task, Comment, Project vb. her şeyin metinlerini içeren düz (flat) bir tablodur.
public class SearchProjection : Entity
{
    public Guid WorkspaceId { get; private set; }
    public Guid EntityId { get; private set; } // TaskId, ProjectId, CommentId vb.
    public string EntityType { get; private set; } = null!; // "Task", "Project" vb.
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!; // Açıklama, Yorum içeriği vb.
    public string? StatusOrState { get; private set; } // Arama sonuçlarında önizleme için
    public string Url { get; private set; } = null!; // Tıklandığında gidilecek URL

    protected SearchProjection() { }

    private SearchProjection(Guid workspaceId, Guid entityId, string entityType, string title, string content, string? statusOrState, string url)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        EntityId = entityId;
        EntityType = entityType;
        Title = title;
        Content = content;
        StatusOrState = statusOrState;
        Url = url;
    }

    public static SearchProjection CreateTaskProjection(TaskItem task, string stateName)
    {
        return new SearchProjection(
            task.WorkspaceId,
            task.Id,
            "Task",
            task.Title,
            task.Description ?? "",
            stateName,
            $"/workspaces/{task.WorkspaceId}/tasks/{task.Id}");
    }

    // PHASE 5 FIX: Added factory method for Documents to be indexed in Global Search
    public static SearchProjection CreateDocumentProjection(Document document)
    {
        return new SearchProjection(
            document.WorkspaceId,
            document.Id,
            "Document",
            document.Title,
            document.Content ?? "",
            document.EmojiIcon, // Using Emoji as Status/Preview for documents
            $"/workspaces/{document.WorkspaceId}/documents?docId={document.Id}" // Simple routing URL
        );
    }

    public void UpdateContent(string title, string content, string? statusOrState)
    {
        Title = title;
        Content = content;
        StatusOrState = statusOrState;
    }
}