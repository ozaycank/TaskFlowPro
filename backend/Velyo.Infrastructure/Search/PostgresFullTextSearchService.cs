using Microsoft.EntityFrameworkCore;
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
        // Arama terimini Postgres Full-Text formatına çeviriyoruz (Örn: doc* | test*)
        string tsQueryStr = string.Join(" | ", searchTerm.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x + ":*"));

        // EF Core'un Shadow Property + Rank + OrderBy çevirisinde yaşadığı 
        // Client-Evaluation çökmesini (500 Error) önlemek için FromSqlInterpolated kullanıyoruz.
        // Bu yaklaşım SQL Injection'a karşı korumalıdır (@p0 parametresi olarak gönderilir) ve GIN index'i kullanır.
        return await _context.Set<SearchProjection>()
            .FromSqlInterpolated($@"
                SELECT * 
                FROM ""SearchProjections"" 
                WHERE ""WorkspaceId"" = {workspaceId} 
                  AND ""SearchVector"" @@ to_tsquery('english', {tsQueryStr})
                ORDER BY ts_rank(""SearchVector"", to_tsquery('english', {tsQueryStr})) DESC
            ")
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}