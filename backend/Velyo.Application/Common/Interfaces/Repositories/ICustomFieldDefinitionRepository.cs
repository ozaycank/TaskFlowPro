using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface ICustomFieldDefinitionRepository
{
    void Add(CustomFieldDefinition definition);
    Task<CustomFieldDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomFieldDefinition>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomFieldDefinition>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    void Delete(CustomFieldDefinition definition);
}