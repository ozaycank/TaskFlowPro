using MediatR;
using TaskFlowPro.Application.Workspaces.Commands.CreateWorkspace;
using TaskFlowPro.Application.Workspaces.Queries.GetWorkspaces; // Added query namespace

namespace TaskFlowPro.Api.Endpoints;

public static class WorkspaceEndpoints
{
    public static void MapWorkspaceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/workspaces").WithTags("Workspaces");

        // POST Endpoint (Existing)
        group.MapPost("/", async (CreateWorkspaceCommand command, IMediator mediator) =>
        {
            var workspaceId = await mediator.Send(command);
            return Results.Created($"/api/workspaces/{workspaceId}", workspaceId);
        })
        .WithName("CreateWorkspace")
        .WithOpenApi();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var query = new GetWorkspacesQuery();
            var workspaces = await mediator.Send(query);
            return Results.Ok(workspaces);
        })
        .WithName("GetWorkspaces")
        .WithOpenApi();
    }
}