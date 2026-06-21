using Microsoft.EntityFrameworkCore;
using NpgsqlTypes; // YENİ: NpgsqlTsVector tipi için eklendi
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Domain.Entities;
using Velyo.Infrastructure.Persistence;

namespace Velyo.Infrastructure.Search;

public class PostgresFullTextSearchService : ISearchService
{
    private readonly ApplicationDbContext _context;

    public PostgresFullTextSearchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SearchProjection>> SearchWorkspaceAsync(Guid workspaceId, string searchTerm, int take = 10, CancellationToken cancellationToken = default)
    {
        // CLEAN ARCHITECTURE FIX: EF.Property ile Gölge Özelliğe (Shadow Property) erişiyoruz.
        return await _context.Set<SearchProjection>()
            .Where(s => s.WorkspaceId == workspaceId &&
                        EF.Property<NpgsqlTsVector>(s, "SearchVector").Matches(EF.Functions.ToTsQuery("english", searchTerm + ":*")))
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}