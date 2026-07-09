using MediatR;
using Velyo.Application.ActivityLogs.Queries.GetActivityLogs;

namespace Velyo.Api.Endpoints;

public static class ActivityEndpoints
{
    public static RouteGroupBuilder MapActivityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/activity").WithTags("Activity").RequireAuthorization();

        group.MapGet("/", async (
            Guid? workspaceId,
            Guid? projectId,
            Guid? taskId,
            Guid? userId,
            int? limit,
            IMediator mediator) =>
        {
            if (workspaceId == null && projectId == null && taskId == null && userId == null)
            {
                return Results.BadRequest("At least one scope (workspaceId, projectId, taskId, userId) must be provided.");
            }

            var result = await mediator.Send(new GetActivityLogsQuery(workspaceId, projectId, taskId, userId, limit ?? 50));
            return Results.Ok(result);
        }).WithName("GetActivityLogs").WithOpenApi();

        return group;
    }
}