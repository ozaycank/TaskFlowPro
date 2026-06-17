using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;

namespace Velyo.Application.Tasks.Commands.ChangeTaskStatus;

public class ChangeTaskStatusCommandHandler : IRequestHandler<ChangeTaskStatusCommand>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeTaskStatusCommandHandler(ITaskItemRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new InvalidOperationException("Task not found.");

        // Domain method encapsulated logic
        task.ChangeStatus(request.NewStatus, request.NewOrderIndex);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}