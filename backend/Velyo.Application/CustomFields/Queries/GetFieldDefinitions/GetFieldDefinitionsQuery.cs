using MediatR;

namespace Velyo.Application.CustomFields.Queries.GetFieldDefinitions;

public record GetFieldDefinitionsQuery(Guid WorkspaceId, Guid? ProjectId) : IRequest<IEnumerable<CustomFieldDefinitionDto>>;