using MediatR;
using Velyo.Application.Sprints.Commands.CompleteSprint;
using Velyo.Application.Sprints.Commands.CreateSprint;
using Velyo.Application.Sprints.Commands.StartSprint;
using Velyo.Application.Sprints.Commands.UpdateSprint;
using Velyo.Application.Sprints.Queries.GetSprintsByProject;

namespace Velyo.Api.Endpoints;

public static class SprintEndpoints
{
    public static RouteGroupBuilder MapSprintEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sprints").WithTags("Sprints").RequireAuthorization();

        group.MapGet("/project/{projectId:guid}", async (Guid projectId, IMediator mediator) =>
        {
            var query = new GetSprintsByProjectQuery(projectId);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        }).WithName("GetSprintsByProject").WithOpenApi();

        group.MapPost("/", async (CreateSprintCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Ok(id);
        }).WithName("CreateSprint").WithOpenApi();

        group.MapPut("/{sprintId:guid}", async (Guid sprintId, UpdateSprintCommand command, IMediator mediator) =>
        {
            if (sprintId != command.SprintId) return Results.BadRequest();
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("UpdateSprint").WithOpenApi();

        group.MapPut("/{sprintId:guid}/start", async (Guid sprintId, IMediator mediator) =>
        {
            await mediator.Send(new StartSprintCommand(sprintId));
            return Results.NoContent();
        }).WithName("StartSprint").WithOpenApi();

        group.MapPut("/{sprintId:guid}/complete", async (Guid sprintId, CompleteSprintCommand command, IMediator mediator) =>
        {
            if (sprintId != command.SprintId) return Results.BadRequest();
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("CompleteSprint").WithOpenApi();

        return group;
    }
}