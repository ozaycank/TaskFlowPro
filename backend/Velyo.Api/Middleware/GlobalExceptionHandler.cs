using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Velyo.Application.Common.Exceptions;

namespace Velyo.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            var validationProblemDetails = new ValidationProblemDetails(validationException.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation error occurred",
                Detail = "One or more validation errors occurred."
            };

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
            return true;
        }

        // For other exceptions (e.g. UnauthorizedAccessException), we can handle them here as well
        if (exception is UnauthorizedAccessException)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = exception.Message
            };
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }

        // Fallback for unhandled exceptions (500)
        var serverErrorDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server error",
            Detail = "An unexpected error occurred."
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(serverErrorDetails, cancellationToken);

        return true;
    }
}