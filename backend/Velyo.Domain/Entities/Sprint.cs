using Velyo.Domain.Common.Models;
using Velyo.Domain.Enums;
using Velyo.Domain.Events;

namespace Velyo.Domain.Entities;

public class Sprint : AuditableEntity
{
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Goal { get; private set; }
    public DateTimeOffset? StartDate { get; private set; }
    public DateTimeOffset? EndDate { get; private set; }
    public SprintStatus Status { get; private set; }

    protected Sprint() { }

    private Sprint(Guid projectId, string name, string? goal, DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Name = name;
        Goal = goal;
        StartDate = startDate;
        EndDate = endDate;
        Status = SprintStatus.Planned;
    }

    public static Sprint Create(Guid projectId, string name, string? goal, DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        var sprint = new Sprint(projectId, name, goal, startDate, endDate);
        sprint.AddDomainEvent(new SprintCreatedEvent(sprint));
        return sprint;
    }

    public void Start()
    {
        if (Status != SprintStatus.Planned)
            throw new InvalidOperationException("Only planned sprints can be started.");

        if (!StartDate.HasValue || !EndDate.HasValue)
            throw new InvalidOperationException("Sprint must have start and end dates to begin.");

        Status = SprintStatus.Active;
        AddDomainEvent(new SprintStartedEvent(this));
    }

    public void Complete()
    {
        if (Status != SprintStatus.Active)
            throw new InvalidOperationException("Only active sprints can be completed.");

        Status = SprintStatus.Completed;
        AddDomainEvent(new SprintCompletedEvent(this));
    }
}