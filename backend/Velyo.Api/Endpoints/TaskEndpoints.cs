using MediatR;
using Velyo.Application.Tasks.Commands.CreateTask;
using Velyo.Application.Tasks.Commands.ChangeTaskStatus;
using Velyo.Application.Tasks.Commands.UpdateTask;
using Velyo.Application.Tasks.Queries.GetTasksByProject;

namespace Velyo.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks").WithTags("Tasks");

        group.MapPost("/", async (CreateTaskCommand command, IMediator mediator) =>
        {
            var taskId = await mediator.Send(command);
            return Results.Created($"/api/tasks/{taskId}", taskId);
        })
        .WithName("CreateTask")
        .WithOpenApi();

        group.MapGet("/project/{projectId:guid}", async (Guid projectId, IMediator mediator) =>
        {
            var query = new GetTasksByProjectQuery(projectId);
            var tasks = await mediator.Send(query);
            return Results.Ok(tasks);
        })
        .WithName("GetTasksByProject")
        .WithOpenApi();

        group.MapPut("/{taskId:guid}/status", async (Guid taskId, ChangeTaskStatusCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch.");
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("ChangeTaskStatus")
        .WithOpenApi();

        group.MapPut("/{taskId:guid}", async (Guid taskId, UpdateTaskCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch.");
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateTask")
        .WithOpenApi();
    }
}