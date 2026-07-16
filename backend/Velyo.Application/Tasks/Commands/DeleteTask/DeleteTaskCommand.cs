using MediatR;

namespace Velyo.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid TaskId) : IRequest;