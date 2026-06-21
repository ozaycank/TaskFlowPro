using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class SearchProjectionRepository : ISearchProjectionRepository
{
    private readonly ApplicationDbContext _context;

    public SearchProjectionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SearchProjection?> GetByEntityIdAsync(Guid entityId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<SearchProjection>()
            .FirstOrDefaultAsync(p => p.EntityId == entityId, cancellationToken);
    }

    public void Add(SearchProjection projection)
    {
        _context.Set<SearchProjection>().Add(projection);
    }

    public void Update(SearchProjection projection)
    {
        _context.Set<SearchProjection>().Update(projection);
    }
}