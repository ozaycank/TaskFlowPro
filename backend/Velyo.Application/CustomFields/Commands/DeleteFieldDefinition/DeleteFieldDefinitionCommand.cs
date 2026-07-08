using MediatR;

namespace Velyo.Application.CustomFields.Commands.DeleteFieldDefinition;

public record DeleteFieldDefinitionCommand(Guid Id) : IRequest;