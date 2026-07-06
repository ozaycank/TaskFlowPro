using MediatR;

namespace Velyo.Application.Worklogs.Commands.DeleteWorklog;

public record DeleteWorklogCommand(Guid WorklogId) : IRequest;