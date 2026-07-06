using MediatR;
using Velyo.Application.Worklogs.Commands.CreateWorklog;
using Velyo.Application.Worklogs.Commands.UpdateWorklog;
using Velyo.Application.Worklogs.Commands.DeleteWorklog;
using Velyo.Application.Worklogs.Queries.GetWorklogsByTask;

namespace Velyo.Api.Endpoints;

public static class TimeTrackingEndpoints
{
    public static RouteGroupBuilder MapTimeTrackingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/time-tracking").WithTags("TimeTracking").RequireAuthorization();

        // GET /api/time-tracking/tasks/{taskId}/worklogs
        group.MapGet("/tasks/{taskId:guid}/worklogs", async (Guid taskId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetWorklogsByTaskQuery(taskId));
            return Results.Ok(result);
        }).WithName("GetWorklogsByTask").WithOpenApi();

        // POST /api/time-tracking/tasks/{taskId}/worklogs
        group.MapPost("/tasks/{taskId:guid}/worklogs", async (Guid taskId, CreateWorklogCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch.");
            var id = await mediator.Send(command);
            return Results.Ok(id);
        }).WithName("CreateWorklog").WithOpenApi();

        // PUT /api/time-tracking/worklogs/{worklogId}
        group.MapPut("/worklogs/{worklogId:guid}", async (Guid worklogId, UpdateWorklogCommand command, IMediator mediator) =>
        {
            if (worklogId != command.WorklogId) return Results.BadRequest("Worklog ID mismatch.");
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateWorklog").WithOpenApi();

        // DELETE /api/time-tracking/worklogs/{worklogId}
        group.MapDelete("/worklogs/{worklogId:guid}", async (Guid worklogId, IMediator mediator) =>
        {
            await mediator.Send(new DeleteWorklogCommand(worklogId));
            return Results.NoContent();
        }).WithName("DeleteWorklog").WithOpenApi();
        
        return group;
    }
}