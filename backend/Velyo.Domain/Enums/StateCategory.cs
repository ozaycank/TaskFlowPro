namespace Velyo.Domain.Enums;

// Linear-style state categorization for high-level reporting regardless of custom naming
public enum StateCategory
{
    Backlog = 10,
    Unstarted = 20,
    Started = 30,
    Completed = 40,
    Canceled = 50
}