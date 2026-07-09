using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Application.ActivityLogs.Commands.LogActivity;

public class LogActivityCommandHandler : IRequestHandler<LogActivityCommand>
{
    private readonly IActivityLogRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public LogActivityCommandHandler(IActivityLogRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(LogActivityCommand request, CancellationToken cancellationToken)
    {
        var log = ActivityLog.Create(
            request.WorkspaceId,
            request.ProjectId,
            request.TaskId,
            request.UserId,
            request.EntityType,
            request.EntityId,
            request.Action,
            request.Details);

        _repository.Add(log);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}