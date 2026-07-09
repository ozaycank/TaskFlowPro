using Microsoft.EntityFrameworkCore;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Infrastructure.Persistence.Repositories;

public class AutomationRuleRepository : IAutomationRuleRepository
{
    private readonly ApplicationDbContext _context;

    public AutomationRuleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(AutomationRule rule) => _context.AutomationRules.Add(rule);
    public void Update(AutomationRule rule) => _context.AutomationRules.Update(rule);
    public void Delete(AutomationRule rule) => _context.AutomationRules.Remove(rule);

    public async Task<AutomationRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.AutomationRules.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<AutomationRule>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
    {
        return await _context.AutomationRules
            .Where(r => r.WorkspaceId == workspaceId && r.ProjectId == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AutomationRule>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return await _context.AutomationRules
            .Where(r => r.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AutomationRule>> GetActiveRulesForTriggerAsync(Guid workspaceId, Guid? projectId, AutomationTriggerType triggerType, CancellationToken cancellationToken = default)
    {
        return await _context.AutomationRules
            .Where(r => r.WorkspaceId == workspaceId
                     && (r.ProjectId == null || r.ProjectId == projectId)
                     && r.TriggerType == triggerType
                     && r.IsActive)
            .ToListAsync(cancellationToken);
    }
}