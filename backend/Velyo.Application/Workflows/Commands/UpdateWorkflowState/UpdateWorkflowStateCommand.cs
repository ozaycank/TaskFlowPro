using MediatR;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workflows.Commands.UpdateWorkflowState;

public record UpdateWorkflowStateCommand(
    Guid WorkflowId,
    Guid StateId,
    string Name,
    string Color,
    StateCategory Category,
    int OrderIndex) : IRequest;