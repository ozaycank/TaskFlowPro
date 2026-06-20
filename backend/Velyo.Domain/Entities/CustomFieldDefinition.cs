using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

public class CustomFieldDefinition : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid? ProjectId { get; private set; } // If null, it's global to the workspace

    public string Name { get; private set; } = null!;
    public FieldType Type { get; private set; }

    // For Select types: JSON array of available options
    public string? OptionsJson { get; private set; }
    public bool IsRequired { get; private set; }

    protected CustomFieldDefinition() { }

    private CustomFieldDefinition(Guid workspaceId, Guid? projectId, string name, FieldType type, string? optionsJson, bool isRequired)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        ProjectId = projectId;
        Name = name;
        Type = type;
        OptionsJson = optionsJson;
        IsRequired = isRequired;
    }

    public static CustomFieldDefinition Create(Guid workspaceId, Guid? projectId, string name, FieldType type, string? optionsJson, bool isRequired)
    {
        return new CustomFieldDefinition(workspaceId, projectId, name, type, optionsJson, isRequired);
    }
}