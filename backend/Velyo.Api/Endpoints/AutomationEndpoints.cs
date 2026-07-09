using MediatR;
using Velyo.Application.Automations.Commands.CreateAutomationRule;
using Velyo.Application.Automations.Commands.DeleteAutomationRule;
using Velyo.Application.Automations.Commands.ToggleAutomationRule;
using Velyo.Application.Automations.Queries.GetAutomationRules;

namespace Velyo.Api.Endpoints;

public static class AutomationEndpoints
{
    public static RouteGroupBuilder MapAutomationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/automations").WithTags("Automations").RequireAuthorization();

        group.MapGet("/workspaces/{workspaceId:guid}", async (Guid workspaceId, Guid? projectId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetAutomationRulesQuery(workspaceId, projectId));
            return Results.Ok(result);
        }).WithName("GetAutomationRules").WithOpenApi();

        group.MapPost("/", async (CreateAutomationRuleCommand command, IMediator mediator) =>
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/automations/{id}", id);
        }).WithName("CreateAutomationRule").WithOpenApi();

        group.MapPatch("/{ruleId:guid}/toggle", async (Guid ruleId, ToggleAutomationRuleCommand command, IMediator mediator) =>
        {
            if (ruleId != command.RuleId) return Results.BadRequest("Rule ID mismatch");
            await mediator.Send(command);
            return Results.NoContent();
        }).WithName("ToggleAutomationRule").WithOpenApi();

        group.MapDelete("/{ruleId:guid}", async (Guid ruleId, IMediator mediator) =>
        {
            await mediator.Send(new DeleteAutomationRuleCommand(ruleId));
            return Results.NoContent();
        }).WithName("DeleteAutomationRule").WithOpenApi();

        return group;
    }
}