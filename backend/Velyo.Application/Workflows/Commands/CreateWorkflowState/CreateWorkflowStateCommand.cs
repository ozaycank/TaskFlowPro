using MediatR;
using Velyo.Domain.Enums;

namespace Velyo.Application.Workflows.Commands.CreateWorkflowState;

public record CreateWorkflowStateCommand(
    Guid WorkflowId,
    string Name,
    string Color,
    StateCategory Category,
    int OrderIndex) : IRequest<Guid>;