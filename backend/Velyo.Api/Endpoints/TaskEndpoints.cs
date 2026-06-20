using MediatR;
using Velyo.Application.Tasks.Commands.CreateTask;
using Velyo.Application.Tasks.Commands.UpdateTask;
using Velyo.Application.Tasks.Queries.GetTasksByProject;
using Velyo.Application.Workflows.Commands.TransitionTaskState;

namespace Velyo.Api.Endpoints;

public static class TaskEndpoints
{
    public static RouteGroupBuilder MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks");

        // POST /api/tasks
        group.MapPost("/", async (CreateTaskCommand command, IMediator mediator) =>
        {
            var taskId = await mediator.Send(command);
            return Results.Created($"/api/tasks/{taskId}", taskId);
        })
        .WithName("CreateTask")
        .WithOpenApi();

        // GET /api/tasks/project/{projectId}
        group.MapGet("/project/{projectId:guid}", async (Guid projectId, IMediator mediator) =>
        {
            var query = new GetTasksByProjectQuery(projectId);
            var tasks = await mediator.Send(query);
            return Results.Ok(tasks);
        })
        .WithName("GetTasksByProject")
        .WithOpenApi();

        // PUT /api/tasks/{taskId}
        group.MapPut("/{taskId:guid}", async (Guid taskId, UpdateTaskCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch.");
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateTask")
        .WithOpenApi();

        // PUT /api/tasks/{taskId}/transition
        group.MapPut("/{taskId:guid}/transition", async (Guid taskId, TransitionTaskStateCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch.");
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("TransitionTaskState")
        .WithOpenApi();

        return group;
    }
}