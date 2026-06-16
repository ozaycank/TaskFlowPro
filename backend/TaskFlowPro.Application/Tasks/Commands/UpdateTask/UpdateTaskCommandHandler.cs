using MediatR;
using TaskFlowPro.Application.Common.Interfaces.Data;
using TaskFlowPro.Application.Common.Interfaces.Repositories;

namespace TaskFlowPro.Application.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskCommandHandler(ITaskItemRepository taskRepository, IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task == null) throw new InvalidOperationException("Task not found.");

        // Domain method encapsulated logic
        task.UpdateDetails(request.Title, request.Description, request.Priority);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}