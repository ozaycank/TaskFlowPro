using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;

namespace Velyo.Domain.Entities;

public class WorkflowState : Entity
{
    public Guid WorkflowId { get; private set; }
    public string Name { get; private set; } = null!; // e.g., "In QA"
    public string Color { get; private set; } = null!; // Hex code for UI
    public StateCategory Category { get; private set; }
    public int OrderIndex { get; private set; }

    protected WorkflowState() { }

    internal WorkflowState(Guid workflowId, string name, string color, StateCategory category, int orderIndex)
    {
        Id = Guid.NewGuid();
        WorkflowId = workflowId;
        Name = name;
        Color = color;
        Category = category;
        OrderIndex = orderIndex;
    }

    internal void UpdateDetails(string name, string color, StateCategory category, int orderIndex)
    {
        Name = name;
        Color = color;
        Category = category;
        OrderIndex = orderIndex;
    }
}