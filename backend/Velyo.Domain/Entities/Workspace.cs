using Velyo.Domain.Common.Models;

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
        return new Workspace(name, description, ownerId);
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}