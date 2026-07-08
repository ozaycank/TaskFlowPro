using MediatR;
using Velyo.Application.CustomFields.Commands.CreateFieldDefinition;
using Velyo.Application.CustomFields.Commands.DeleteFieldDefinition;
using Velyo.Application.CustomFields.Commands.UpdateFieldDefinition;
using Velyo.Application.CustomFields.Commands.SetTaskFieldValue;
using Velyo.Application.CustomFields.Queries.GetFieldDefinitions;

namespace Velyo.Api.Endpoints;

public static class CustomFieldEndpoints
{
    public static RouteGroupBuilder MapCustomFieldEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/custom-fields").WithTags("CustomFields").RequireAuthorization();

        group.MapGet("/workspaces/{workspaceId:guid}", async (Guid workspaceId, Guid? projectId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetFieldDefinitionsQuery(workspaceId, projectId));
            return Results.Ok(result);
        }).WithName("GetFieldDefinitions").WithOpenApi();

        group.MapPost("/", async (CreateFieldDefinitionCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/custom-fields/{id}", id);
        }).WithName("CreateFieldDefinition").WithOpenApi();

        group.MapPut("/{id:guid}", async (Guid id, UpdateFieldDefinitionCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateFieldDefinition").WithOpenApi();

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteFieldDefinitionCommand(id));
            return Results.NoContent();
        }).WithName("DeleteFieldDefinition").WithOpenApi();

        // Values
        group.MapPut("/tasks/{taskId:guid}/values", async (Guid taskId, SetTaskFieldValueCommand command, IMediator mediator) =>
        {
            if (taskId != command.TaskId) return Results.BadRequest("Task ID mismatch");
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("SetTaskFieldValue").WithOpenApi();

        return group;
    }
}