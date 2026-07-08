using MediatR;

namespace Velyo.Application.CustomFields.Commands.UpdateFieldDefinition;

public record UpdateFieldDefinitionCommand(Guid Id, string Name, string? OptionsJson, bool IsRequired) : IRequest;