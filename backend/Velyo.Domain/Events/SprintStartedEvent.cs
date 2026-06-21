using Velyo.Domain.Common.Models;
using Velyo.Domain.Entities;

namespace Velyo.Domain.Events;

public class SprintStartedEvent : DomainEvent
{
    public Sprint Sprint { get; }
    public SprintStartedEvent(Sprint sprint) => Sprint = sprint;
}