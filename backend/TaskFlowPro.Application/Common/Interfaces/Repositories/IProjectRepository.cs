using TaskFlowPro.Domain.Entities;

namespace TaskFlowPro.Application.Common.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Project>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    void Add(Project project);
    void Update(Project project);
    void Delete(Project project);
}