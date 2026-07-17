using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velyo.Application.Workflows.Commands.CreateWorkflowState;
using Velyo.Application.Workflows.Commands.DeleteWorkflowState;
using Velyo.Application.Workflows.Commands.TransitionTaskState;
using Velyo.Application.Workflows.Commands.UpdateWorkflowState;
using Velyo.Application.Workflows.Queries.GetWorkflowStates;

namespace Velyo.Api.Endpoints;

public static class WorkflowEndpoints
{
    public static RouteGroupBuilder MapWorkflowEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workflows").WithTags("Workflows");

        // GET /api/workflows/workspaces/{workspaceId}/states
        group.MapGet("/workspaces/{workspaceId:guid}/states", async (Guid workspaceId, IMediator mediator) =>
        {
            var query = new GetWorkflowStatesQuery(workspaceId);
            var states = await mediator.Send(query);
            return Results.Ok(states);
        })
        .WithName("GetWorkflowStates")
        .WithOpenApi();

        // POST /api/workflows/{workflowId}/states
        group.MapPost("/{workflowId:guid}/states", async (Guid workflowId, CreateWorkflowStateCommand command, IMediator mediator) =>
        {
            if (workflowId != command.WorkflowId) return Results.BadRequest("Workflow ID mismatch.");
            var stateId = await mediator.Send(command);
            return Results.Created($"/api/workflows/{workflowId}/states/{stateId}", new { id = stateId });
        })
        .WithName("CreateWorkflowState")
        .WithOpenApi();

        // PUT /api/workflows/{workflowId}/states/{stateId}
        group.MapPut("/{workflowId:guid}/states/{stateId:guid}", async (Guid workflowId, Guid stateId, UpdateWorkflowStateCommand command, IMediator mediator) =>
        {
            if (workflowId != command.WorkflowId || stateId != command.StateId)
                return Results.BadRequest("ID mismatch.");

            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateWorkflowState")
        .WithOpenApi();

        // DELETE /api/workflows/{workflowId}/states/{stateId}
        group.MapDelete("/{workflowId:guid}/states/{stateId:guid}", async (Guid workflowId, Guid stateId, IMediator mediator) =>
        {
            await mediator.Send(new DeleteWorkflowStateCommand(workflowId, stateId));
            return Results.NoContent();
        })
        .WithName("DeleteWorkflowState")
        .WithOpenApi();

        return group;
    }
}