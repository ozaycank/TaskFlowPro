using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velyo.Application.Projects.Commands.CreateProject;
using Velyo.Application.Projects.Commands.UpdateProject;
using Velyo.Application.Projects.Commands.DeleteProject;
using Velyo.Application.Projects.Queries.GetProjects;

namespace Velyo.Api.Endpoints;

public static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").WithTags("Projects");

        // GET: All projects for a workspace
        group.MapGet("/", async ([FromQuery] Guid workspaceId, IMediator mediator) =>
        {
            var query = new GetProjectsQuery(workspaceId);
            var projects = await mediator.Send(query);
            return Results.Ok(projects);
        })
        .WithName("GetProjects")
        .WithOpenApi();

        // GET: Single project by ID
        group.MapGet("/{id:guid}", async (Guid id, [FromQuery] Guid workspaceId, IMediator mediator) =>
        {
            var query = new GetProjectsQuery(workspaceId);
            var projects = await mediator.Send(query);
            var project = projects.FirstOrDefault(p => p.Id == id);

            return project != null ? Results.Ok(project) : Results.NotFound();
        })
        .WithName("GetProjectById")
        .WithOpenApi();

        // POST: Create a new project
        group.MapPost("/", async (CreateProjectCommand command, IMediator mediator) =>
        {
            var projectId = await mediator.Send(command);
            return Results.Created($"/api/projects/{projectId}", new { id = projectId });
        })
        .WithName("CreateProject")
        .WithOpenApi();

        // PUT: Update an existing project
        group.MapPut("/{id:guid}", async (Guid id, UpdateProjectCommand command, IMediator mediator) =>
        {
            if (id != command.ProjectId) return Results.BadRequest("Project ID mismatch.");

            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateProject")
        .WithOpenApi();

        // DELETE: Delete a project
        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            await mediator.Send(new DeleteProjectCommand(id));
            return Results.NoContent();
        })
        .WithName("DeleteProject")
        .WithOpenApi();

        return group;
    }
}