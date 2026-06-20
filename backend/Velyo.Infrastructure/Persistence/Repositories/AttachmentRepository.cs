using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly ApplicationDbContext _context;

    public AttachmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Attachment attachment)
    {
        _context.Attachments.Add(attachment);
    }

    public async Task<Attachment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Attachments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Attachment>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return await _context.Attachments
            .Where(a => a.TaskId == taskId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public void Delete(Attachment attachment)
    {
        _context.Attachments.Remove(attachment);
    }
}