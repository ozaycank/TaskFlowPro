using MediatR;

namespace Velyo.Application.Worklogs.Commands.UpdateWorklog;

public record UpdateWorklogCommand(
    Guid WorklogId,
    long TimeSpentSeconds,
    DateTimeOffset StartDate,
    string? Description) : IRequest;