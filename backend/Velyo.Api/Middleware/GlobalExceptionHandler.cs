using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Velyo.Application.Common.Exceptions;

namespace Velyo.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = CreateProblemDetails(httpContext, exception);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ValidationProblemDetails(validationException.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Instance = httpContext.Request.Path
            },
            NotFoundException notFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Resource Not Found",
                Detail = notFoundException.Message,
                Instance = httpContext.Request.Path
            },
            UnauthorizedAccessException unauthorizedException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Title = "Unauthorized",
                Detail = unauthorizedException.Message,
                Instance = httpContext.Request.Path
            },
            ForbiddenAccessException forbiddenException => new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Title = "Forbidden",
                Detail = forbiddenException.Message,
                Instance = httpContext.Request.Path
            },
            // Default Fallback
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred while processing your request.",
                Instance = httpContext.Request.Path
            }
        };
    }
}