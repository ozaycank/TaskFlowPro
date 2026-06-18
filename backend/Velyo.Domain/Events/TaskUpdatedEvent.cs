using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class TaskUpdatedEvent : DomainEvent
{
    public TaskItem Task { get; }
    public Guid InitiatedByUserId { get; }

    public TaskUpdatedEvent(TaskItem task, Guid initiatedByUserId)
    {
        Task = task;
        InitiatedByUserId = initiatedByUserId;
    }
}