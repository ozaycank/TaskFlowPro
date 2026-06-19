using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Comment comment)
    {
        _context.Comments.Add(comment);
    }

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        return await _context.Comments
            .Where(c => c.TaskId == taskId)
            .OrderBy(c => c.CreatedAt) // Yorumlar eskiden yeniye sıralanmalı
            .ToListAsync(cancellationToken);
    }

    public void Delete(Comment comment)
    {
        _context.Comments.Remove(comment);
    }
}