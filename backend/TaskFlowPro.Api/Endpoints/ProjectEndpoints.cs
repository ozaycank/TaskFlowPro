using MediatR;
using TaskFlowPro.Application.Projects.Commands.CreateProject;
using TaskFlowPro.Application.Projects.Queries.GetProjects;

namespace TaskFlowPro.Api.Endpoints;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").WithTags("Projects");

        // POST Endpoint
        group.MapPost("/", async (CreateProjectCommand command, IMediator mediator) =>
        {
            var projectId = await mediator.Send(command);
            return Results.Created($"/api/projects/{projectId}", projectId);
        })
        .WithName("CreateProject")
        .WithOpenApi();

        // GET Endpoint
        group.MapGet("/{workspaceId:guid}", async (Guid workspaceId, IMediator mediator) =>
        {
            var query = new GetProjectsQuery(workspaceId);
            var projects = await mediator.Send(query);
            return Results.Ok(projects);
        })
        .WithName("GetProjects")
        .WithOpenApi();
    }
}