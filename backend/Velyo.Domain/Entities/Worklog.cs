using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class Worklog : AuditableEntity
{
    public Guid WorkspaceId { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid UserId { get; private set; }
    public long TimeSpentSeconds { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public string? Description { get; private set; }

    protected Worklog() { }

    private Worklog(Guid workspaceId, Guid taskId, Guid userId, long timeSpentSeconds, DateTimeOffset startDate, string? description)
    {
        Id = Guid.NewGuid();
        WorkspaceId = workspaceId;
        TaskId = taskId;
        UserId = userId;
        TimeSpentSeconds = timeSpentSeconds;
        StartDate = startDate;
        Description = description;
    }

    public static Worklog Create(Guid workspaceId, Guid taskId, Guid userId, long timeSpentSeconds, DateTimeOffset startDate, string? description)
    {
        if (timeSpentSeconds <= 0) throw new ArgumentException("Time spent must be greater than zero.");
        return new Worklog(workspaceId, taskId, userId, timeSpentSeconds, startDate, description);
    }

    public void Update(long timeSpentSeconds, DateTimeOffset startDate, string? description)
    {
        if (timeSpentSeconds <= 0) throw new ArgumentException("Time spent must be greater than zero.");
        TimeSpentSeconds = timeSpentSeconds;
        StartDate = startDate;
        Description = description;
    }
}