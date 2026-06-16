using MediatR;
using TaskFlowPro.Application.Common.Interfaces.Data;
using TaskFlowPro.Application.Common.Interfaces.Repositories;
using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskItemRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskCommandHandler(
        ITaskItemRepository taskRepository,
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // Business Rule: Validate Project existence
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
        if (project == null || project.WorkspaceId != request.WorkspaceId)
        {
            throw new InvalidOperationException("Project not found or does not belong to the specified workspace.");
        }

        var task = TaskItem.Create(
            request.WorkspaceId,
            request.ProjectId,
            request.Title,
            request.Description,
            request.Priority,
            request.OrderIndex);

        _taskRepository.Add(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return task.Id;
    }
}