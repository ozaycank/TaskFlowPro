using MediatR;

namespace TaskFlowPro.Application.Workspaces.Commands.CreateWorkspace;

// Returns a Guid which will be the ID of the newly created workspace
public record CreateWorkspaceCommand(string Name, string? Description) : IRequest<Guid>;