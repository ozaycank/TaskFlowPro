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

        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Title = "Validation Failed";
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Detail = "One or more validation errors occurred.";
            problemDetails.Extensions.Add("errors", validationException.Errors);
        }
        else if (exception is NotFoundException notFoundException)
        {
            problemDetails.Title = "Resource Not Found";
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Detail = notFoundException.Message;
        }
        // YENİ EKLENEN KISIM: Authorization hatalarını zarifçe 403 döndür
        else if (exception is ForbiddenAccessException)
        {
            problemDetails.Title = "Forbidden";
            problemDetails.Status = StatusCodes.Status403Forbidden;
            problemDetails.Detail = "You do not have permission to access or modify this resource.";
        }
        else if (exception is UnauthorizedAccessException)
        {
            problemDetails.Title = "Unauthorized";
            problemDetails.Status = StatusCodes.Status401Unauthorized;
            problemDetails.Detail = exception.Message; // "Invalid credentials."
        }
        else
        {
            problemDetails.Title = "Server Error";
            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Detail = "An unexpected error occurred.";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}