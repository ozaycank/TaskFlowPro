using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class OutboxMessage : Entity
{
    public string Type { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public DateTimeOffset OccurredOn { get; private set; }
    public DateTimeOffset? ProcessedOn { get; private set; }
    public string? Error { get; private set; }

    protected OutboxMessage() { }

    public static OutboxMessage Create(string type, string content)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Content = content,
            OccurredOn = DateTimeOffset.UtcNow
        };
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTimeOffset.UtcNow;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
    }
}