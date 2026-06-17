using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class Project : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }

    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    protected Project() { }

    private Project(Guid workspaceId, string name, string? description)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        Name = name;
        Description = description;
    }

    public static Project Create(Guid workspaceId, string name, string? description)
    {
        return new Project(workspaceId, name, description);
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}