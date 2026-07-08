using Velyo.Domain.Enums;

namespace Velyo.Application.CustomFields.Queries.GetFieldDefinitions;

public record CustomFieldDefinitionDto(
    Guid Id,
    Guid WorkspaceId,
    Guid? ProjectId,
    string Name,
    FieldType Type,
    string? OptionsJson,
    bool IsRequired);