using MediatR;
using Microsoft.Extensions.Logging;
using TaskFlowPro.Application.Common.Interfaces.Services;

namespace TaskFlowPro.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.UserId ?? "Unauthenticated";

        _logger.LogInformation("TaskFlowPro Request: {Name} [User: {UserId}] {@Request}",
            requestName, userId, request);

        var response = await next();

        _logger.LogInformation("TaskFlowPro Response: {Name} [User: {UserId}]",
            requestName, userId);

        return response;
    }
}