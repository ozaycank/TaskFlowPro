using MediatR;
using Velyo.Application.Tasks.Commands.CreateTask;
using Velyo.Application.Tasks.Commands.UpdateTask;
using Velyo.Application.Tasks.Queries.GetTasksByProject;
using Velyo.Application.Workflows.Commands.TransitionTaskState;
using Velyo.Application.Comments.Commands.CreateComment;
using Velyo.Application.Comments.Queries.GetCommentsByTask;
using Velyo.Application.Attachments.Commands.RequestUploadUrl;
using Velyo.Application.Attachments.Commands.CompleteUpload;
using Velyo.Application.Attachments.Queries.GetAttachmentsByTask;

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
        // GET /api/tasks/{taskId}
        group.MapGet("/{taskId:guid}", async (Guid taskId, IMediator mediator) =>
        {
            var query = new Velyo.Application.Tasks.Queries.GetTaskById.GetTaskByIdQuery(taskId);
            var task = await mediator.Send(query);
            return Results.Ok(task);
        })
        .WithName("GetTaskById")
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
        
        group.MapPost("/{taskId:guid}/comments", async (Guid taskId, CreateCommentCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch.");
            var commentId = await mediator.Send(command);
            return Results.Created($"/api/tasks/{taskId}/comments/{commentId}", commentId);
        })
        .WithName("CreateComment")
        .WithOpenApi();

        group.MapGet("/{taskId:guid}/comments", async (Guid taskId, IMediator mediator) =>
        {
            var query = new GetCommentsByTaskQuery(taskId);
            var comments = await mediator.Send(query);
            return Results.Ok(comments);
        })
        .WithName("GetTaskComments")
        .WithOpenApi();

        // --- ATTACHMENTS ---
        group.MapPost("/{taskId:guid}/attachments/request-upload", async (Guid taskId, RequestUploadUrlCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch.");
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("RequestAttachmentUploadUrl")
        .WithOpenApi();

        group.MapPost("/{taskId:guid}/attachments/complete-upload", async (Guid taskId, CompleteUploadCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("CompleteAttachmentUpload")
        .WithOpenApi();

        group.MapGet("/{taskId:guid}/attachments", async (Guid taskId, IMediator mediator) =>
        {
            var query = new GetAttachmentsByTaskQuery(taskId);
            var attachments = await mediator.Send(query);
            return Results.Ok(attachments);
        })
        .WithName("GetTaskAttachments")
        .WithOpenApi();

        return group;
    }
}