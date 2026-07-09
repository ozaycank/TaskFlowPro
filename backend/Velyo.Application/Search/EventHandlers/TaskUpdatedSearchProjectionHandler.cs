using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;
using Velyo.Domain.Events;

namespace Velyo.Application.Search.EventHandlers;

public class TaskUpdatedSearchProjectionHandler :
    INotificationHandler<TaskStateTransitionedEvent>,
    INotificationHandler<TaskAssignedToSprintEvent>
{
    private readonly ISearchProjectionRepository _searchProjectionRepository;
    private readonly ITaskItemRepository _taskItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskUpdatedSearchProjectionHandler(
        ISearchProjectionRepository searchProjectionRepository,
        ITaskItemRepository taskItemRepository,
        IUnitOfWork unitOfWork)
    {
        _searchProjectionRepository = searchProjectionRepository;
        _taskItemRepository = taskItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TaskStateTransitionedEvent notification, CancellationToken cancellationToken)
    {
        // Kuyruktan gelen eksik nesne yerine sadece ID'yi gönderiyoruz
        await UpdateProjectionAsync(notification.Task.Id, cancellationToken);
    }

    public async Task Handle(TaskAssignedToSprintEvent notification, CancellationToken cancellationToken)
    {
        await UpdateProjectionAsync(notification.Task.Id, cancellationToken);
    }

    private async Task UpdateProjectionAsync(Guid taskId, CancellationToken cancellationToken)
    {
        // 1. Veritabanından Task'ın en güncel ve FULL halini çekiyoruz (Outbox Deserialization null sorununu aşmak için)
        var task = await _taskItemRepository.GetByIdAsync(taskId, cancellationToken);

        if (task == null) return; // Task silinmişse güncellemeye gerek yok

        var projection = await _searchProjectionRepository.GetByEntityIdAsync(task.Id, cancellationToken);

        // Güvenlik: Null olabilecek verileri temizle
        var safeTitle = string.IsNullOrWhiteSpace(task.Title) ? "Untitled Task" : task.Title;
        var safeDescription = task.Description ?? "";
        var stateStr = task.StateId.ToString();

        if (projection == null)
        {
            projection = SearchProjection.CreateTaskProjection(task, stateStr);
            _searchProjectionRepository.Add(projection);
        }
        else
        {
            projection.UpdateContent(safeTitle, safeDescription, stateStr);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}