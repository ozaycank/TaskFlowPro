namespace Velyo.Domain.Enums;

public enum SprintStatus
{
    Planned = 10,  // Ready in backlog
    Active = 20,   // Currently running sprint
    Completed = 30, // Finished sprint
    Closed = 40    // Archived/Closed
}