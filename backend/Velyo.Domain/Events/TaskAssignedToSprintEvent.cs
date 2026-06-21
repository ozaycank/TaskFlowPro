using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class TaskAssignedToSprintEvent : DomainEvent
{
    public TaskItem Task { get; }
    public Guid? OldSprintId { get; }
    public Guid? NewSprintId { get; }

    public TaskAssignedToSprintEvent(TaskItem task, Guid? oldSprintId, Guid? newSprintId)
    {
        Task = task;
        OldSprintId = oldSprintId;
        NewSprintId = newSprintId;
    }
}