using Serilog.Context;
using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Api.Middleware;

public class RequestLogContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLogContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
    {
        using (LogContext.PushProperty("UserId", currentUserService.UserId ?? "Anonymous"))
        using (LogContext.PushProperty("UserEmail", currentUserService.Email ?? "None"))
        using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
        {
            await _next(context);
        }
    }
}