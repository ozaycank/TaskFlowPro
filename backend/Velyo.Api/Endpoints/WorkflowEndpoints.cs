using MediatR;
using Velyo.Application.Workflows.Commands.TransitionTaskState;
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

        return group;
    }
}