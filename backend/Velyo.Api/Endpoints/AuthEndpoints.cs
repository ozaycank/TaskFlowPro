using MediatR;
using Microsoft.AspNetCore.Http;
using Velyo.Application.Auth.Commands.Login;
using Velyo.Application.Auth.Commands.Refresh;
using Velyo.Application.Auth.Commands.Register;

namespace Velyo.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/register", async (RegisterCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).AllowAnonymous();

        group.MapPost("/login", async (LoginCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        }).AllowAnonymous();

        group.MapPost("/refresh", async (HttpContext context, IMediator mediator) =>
        {
            // The frontend proxy.ts / Axios Interceptor places the refreshToken in a secure cookie
            if (!context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            {
                return Results.Unauthorized();
            }

            try
            {
                var command = new RefreshCommand(refreshToken);
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
        }).AllowAnonymous();

        return group;
    }
}