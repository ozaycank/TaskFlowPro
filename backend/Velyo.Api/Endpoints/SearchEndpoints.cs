using MediatR;
using Velyo.Application.Search.Queries.GlobalSearch;

namespace Velyo.Api.Endpoints;

public static class SearchEndpoints
{
    public static RouteGroupBuilder MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/search").WithTags("Search").RequireAuthorization();

        // GET /api/search?workspaceId={guid}&q={term}
        group.MapGet("/", async (Guid workspaceId, string q, IMediator mediator) =>
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return Results.BadRequest("Search term cannot be empty.");
            }

            var query = new GlobalSearchQuery(workspaceId, q);
            var results = await mediator.Send(query);

            return Results.Ok(results);
        }).WithName("GlobalSearch").WithOpenApi();

        return group;
    }
}