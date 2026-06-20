using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IAttachmentRepository
{
    void Add(Attachment attachment);
    Task<Attachment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Attachment>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default);
    void Delete(Attachment attachment);
}