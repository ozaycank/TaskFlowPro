using MediatR;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories; // YENİ: Repository interface eklendi
using Velyo.Domain.Entities;
using Velyo.Domain.Events;

namespace Velyo.Application.Search.EventHandlers;

public class TaskUpdatedSearchProjectionHandler :
    INotificationHandler<TaskStateTransitionedEvent>,
    INotificationHandler<TaskAssignedToSprintEvent>
{
    // YENİ: DbContext yerine Interface kullanıyoruz
    private readonly ISearchProjectionRepository _searchProjectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskUpdatedSearchProjectionHandler(ISearchProjectionRepository searchProjectionRepository, IUnitOfWork unitOfWork)
    {
        _searchProjectionRepository = searchProjectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(TaskStateTransitionedEvent notification, CancellationToken cancellationToken)
    {
        await UpdateProjectionAsync(notification.Task, cancellationToken);
    }

    public async Task Handle(TaskAssignedToSprintEvent notification, CancellationToken cancellationToken)
    {
        await UpdateProjectionAsync(notification.Task, cancellationToken);
    }

    private async Task UpdateProjectionAsync(TaskItem task, CancellationToken cancellationToken)
    {
        // YENİ: DbContext yerine Repository üzerinden sorgu yapıyoruz
        var projection = await _searchProjectionRepository.GetByEntityIdAsync(task.Id, cancellationToken);

        if (projection == null)
        {
            projection = SearchProjection.CreateTaskProjection(task, task.StateId.ToString());
            _searchProjectionRepository.Add(projection);
        }
        else
        {
            projection.UpdateContent(task.Title, task.Description ?? "", task.StateId.ToString());
            // Entity Framework Track ettiği için Update metodunu çağırmak opsiyoneldir ancak
            // CQRS mimarilerinde okunabilirliği artırır.
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}