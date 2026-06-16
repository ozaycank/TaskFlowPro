using MediatR;

namespace TaskFlowPro.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(Guid WorkspaceId, string Name, string? Description) : IRequest<Guid>;