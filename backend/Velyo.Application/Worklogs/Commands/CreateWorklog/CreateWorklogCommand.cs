using MediatR;

namespace Velyo.Application.Worklogs.Commands.CreateWorklog;

public record CreateWorklogCommand(
    Guid TaskId,
    Guid UserId, // In a real app, infer this from HttpContext/CurrentUserService
    long TimeSpentSeconds,
    DateTimeOffset StartDate,
    string? Description) : IRequest<Guid>;