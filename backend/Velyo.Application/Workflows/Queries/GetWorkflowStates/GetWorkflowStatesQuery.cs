using MediatR;

namespace Velyo.Application.Workflows.Queries.GetWorkflowStates;

// Çalışma alanına (Workspace) ait tüm dinamik iş akışı durumlarını (Kanban sütunlarını) getirir.
public record GetWorkflowStatesQuery(Guid WorkspaceId) : IRequest<IEnumerable<WorkflowStateDto>>;