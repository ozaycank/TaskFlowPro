using MediatR;
using Velyo.Application.Analytics.Queries.GetBurndown;
using Velyo.Application.Analytics.Queries.GetCumulativeFlow;
using Velyo.Application.Analytics.Queries.GetCycleTime;
using Velyo.Application.Analytics.Queries.GetSprintVelocity;

namespace Velyo.Api.Endpoints;

public static class AnalyticsEndpoints
{
    public static RouteGroupBuilder MapAnalyticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/analytics").WithTags("Analytics").RequireAuthorization();

        group.MapGet("/projects/{projectId:guid}/velocity", async (Guid projectId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSprintVelocityQuery(projectId));
            return Results.Ok(result);
        }).WithName("GetSprintVelocity").WithOpenApi();

        group.MapGet("/sprints/{sprintId:guid}/burndown", async (Guid sprintId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetBurndownQuery(sprintId));
            return Results.Ok(result);
        }).WithName("GetSprintBurndown").WithOpenApi();

        group.MapGet("/projects/{projectId:guid}/cycle-time", async (Guid projectId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCycleTimeQuery(projectId));
            return Results.Ok(result);
        }).WithName("GetCycleTime").WithOpenApi();

        group.MapGet("/projects/{projectId:guid}/cumulative-flow", async (Guid projectId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCumulativeFlowQuery(projectId));
            return Results.Ok(result);
        }).WithName("GetCumulativeFlow").WithOpenApi();

        return group;
    }
}