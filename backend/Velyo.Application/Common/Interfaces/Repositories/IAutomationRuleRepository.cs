using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IAutomationRuleRepository
{
    void Add(AutomationRule rule);
    void Update(AutomationRule rule);
    void Delete(AutomationRule rule);
    Task<AutomationRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AutomationRule>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AutomationRule>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    // Used internally by the execution engine to find rules matching a trigger
    Task<IEnumerable<AutomationRule>> GetActiveRulesForTriggerAsync(Guid workspaceId, Guid? projectId, AutomationTriggerType triggerType, CancellationToken cancellationToken = default);
}