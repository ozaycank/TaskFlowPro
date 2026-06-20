using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class TaskStateTransitionedEvent : DomainEvent
{
    public TaskItem Task { get; }
    public Guid NewStateId { get; }

    public TaskStateTransitionedEvent(TaskItem task, Guid newStateId)
    {
        Task = task;
        NewStateId = newStateId;
    }
}