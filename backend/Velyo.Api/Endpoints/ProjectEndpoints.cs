using MediatR;
using Microsoft.AspNetCore.Mvc;
using Velyo.Application.Projects.Commands.CreateProject;
using Velyo.Application.Projects.Queries.GetProjects;

namespace Velyo.Api.Endpoints;

public static class ProjectEndpoints
{
    public static RouteGroupBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects").WithTags("Projects");

        // GET: Tüm Projeleri Workspace'e Göre Getir (Query Param)
        group.MapGet("/", async ([FromQuery] Guid workspaceId, IMediator mediator) =>
        {
            var query = new GetProjectsQuery(workspaceId);
            var projects = await mediator.Send(query);
            return Results.Ok(projects);
        })
        .WithName("GetProjects")
        .WithOpenApi();

        // GET: Tek Bir Projeyi ID'sine Göre Getir (Route Param)
        group.MapGet("/{id:guid}", async (Guid id, [FromQuery] Guid workspaceId, IMediator mediator) =>
        {
            // Eğer GetProjectByIdQuery adında bir CQRS nesneniz yoksa, şimdilik GetProjectsQuery kullanıp bellek içi filtreleme yapalım:
            var query = new GetProjectsQuery(workspaceId);
            var projects = await mediator.Send(query);
            var project = projects.FirstOrDefault(p => p.Id == id);

            return project != null ? Results.Ok(project) : Results.NotFound();
        })
        .WithName("GetProjectById")
        .WithOpenApi();

        // POST: Yeni Proje Oluştur
        group.MapPost("/", async (CreateProjectCommand command, IMediator mediator) =>
        {
            var projectId = await mediator.Send(command);
            return Results.Created($"/api/projects/{projectId}", new { id = projectId });
        })
        .WithName("CreateProject")
        .WithOpenApi();

        return group;
    }
}