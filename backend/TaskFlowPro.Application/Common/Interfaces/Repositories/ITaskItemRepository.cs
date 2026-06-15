using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Application.Common.Interfaces.Repositories;

public interface ITaskItemRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskItem>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    void Add(TaskItem taskItem);
    void Update(TaskItem taskItem);
    void Delete(TaskItem taskItem);
}