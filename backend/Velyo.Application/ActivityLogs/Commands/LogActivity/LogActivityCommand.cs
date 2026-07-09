using MediatR;

namespace Velyo.Application.ActivityLogs.Commands.LogActivity;

public record LogActivityCommand(
    Guid WorkspaceId,
    Guid? ProjectId,
    Guid? TaskId,
    Guid UserId,
    string EntityType,
    Guid EntityId,
    string Action,
    string? Details = null) : IRequest;