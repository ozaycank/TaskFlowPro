using MediatR;
using Velyo.Application.Common.Exceptions;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;

namespace Velyo.Application.Worklogs.Commands.CreateWorklog;

public class CreateWorklogCommandHandler : IRequestHandler<CreateWorklogCommand, Guid>
{
    private readonly IWorklogRepository _worklogRepository;
    private readonly ITaskItemRepository _taskRepository;
    private readonly IWorkspaceAuthorizationService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorklogCommandHandler(IWorklogRepository worklogRepository, ITaskItemRepository taskRepository, IWorkspaceAuthorizationService authService, IUnitOfWork unitOfWork)
    {
        _worklogRepository = worklogRepository;
        _taskRepository = taskRepository;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateWorklogCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new NotFoundException(nameof(TaskItem), request.TaskId);

        await _authService.AuthorizeMembershipAsync(task.WorkspaceId, cancellationToken);

        var worklog = Worklog.Create(task.WorkspaceId, request.TaskId, request.UserId, request.TimeSpentSeconds, request.StartDate, request.Description);

        _worklogRepository.Add(worklog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return worklog.Id;
    }
}