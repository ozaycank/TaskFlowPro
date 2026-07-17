using MediatR;

namespace Velyo.Application.Workflows.Commands.DeleteWorkflowState;

public record DeleteWorkflowStateCommand(Guid WorkflowId, Guid StateId) : IRequest;