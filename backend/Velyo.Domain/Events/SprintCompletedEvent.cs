using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class SprintCompletedEvent : DomainEvent
{
    public Sprint Sprint { get; }
    public SprintCompletedEvent(Sprint sprint) => Sprint = sprint;
}