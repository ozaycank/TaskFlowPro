using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class SprintRepository : ISprintRepository
{
    private readonly ApplicationDbContext _context;

    public SprintRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(Sprint sprint)
    {
        _context.Sprints.Add(sprint);
    }

    public void Update(Sprint sprint)
    {
        _context.Sprints.Update(sprint);
    }

    public async Task<Sprint?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sprints.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Sprint>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _context.Sprints
            .Where(s => s.ProjectId == projectId)
            .OrderBy(s => s.StartDate)
            .ToListAsync(cancellationToken);
    }
}