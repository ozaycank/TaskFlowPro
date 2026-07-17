using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class Workflow : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsDefault { get; private set; }

    private readonly List<WorkflowState> _states = new();
    public IReadOnlyCollection<WorkflowState> States => _states.AsReadOnly();

    protected Workflow() { }

    private Workflow(Guid workspaceId, string name, bool isDefault)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        Name = name;
        IsDefault = isDefault;
    }

    public static Workflow Create(Guid workspaceId, string name, bool isDefault = false)
    {
        return new Workflow(workspaceId, name, isDefault);
    }

    public WorkflowState AddState(string name, string color, Enums.StateCategory category, int orderIndex)
    {
        var state = new WorkflowState(Id, name, color, category, orderIndex);
        _states.Add(state);
        return state;
    }

    public void UpdateState(Guid stateId, string name, string color, Enums.StateCategory category, int orderIndex)
    {
        var state = _states.FirstOrDefault(s => s.Id == stateId);
        if (state != null)
        {
            state.UpdateDetails(name, color, category, orderIndex);
        }
    }

    public void RemoveState(Guid stateId)
    {
        var state = _states.FirstOrDefault(s => s.Id == stateId);
        if (state != null)
        {
            _states.Remove(state);
        }
    }
}