using Velyo.Domain.Common.Models;
using Velyo.Domain.Events; // YENİ: Application değil Domain Event referansı

namespace Velyo.Domain.Entities;

public class Workspace : AuditableEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }

    protected Workspace() { }

    private Workspace(string name, string? description, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }

    public static Workspace Create(string name, string? description, Guid ownerId)
    {
        var workspace = new Workspace(name, description, ownerId);

        workspace.AddDomainEvent(new WorkspaceCreatedEvent(workspace, ownerId));

        return workspace;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}