namespace Velyo.Application.Workflows.Queries.GetWorkflowStates;

public record WorkflowStateDto(
    Guid Id,
    string Name,
    string Color,
    float OrderIndex);