using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class SprintCreatedEvent : DomainEvent
{
    public Sprint Sprint { get; }
    public SprintCreatedEvent(Sprint sprint) => Sprint = sprint;
}