using TaskFlowPro.Application.Common.Interfaces.Data;

namespace TaskFlowPro.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // In the future, this is where we will dispatch Domain Events 
        // before or after saving changes to the database.
        
        return await _context.SaveChangesAsync(cancellationToken);
    }
}