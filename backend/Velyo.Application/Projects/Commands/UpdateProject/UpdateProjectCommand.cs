using MediatR;

namespace Velyo.Application.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(Guid ProjectId, string Name, string? Description) : IRequest;