using MediatR;

namespace Velyo.Application.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid ProjectId) : IRequest;