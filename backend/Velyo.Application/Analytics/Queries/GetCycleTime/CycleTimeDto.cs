namespace Velyo.Application.Analytics.Queries.GetCycleTime;

public record CycleTimeDto(
    Guid TaskId,
    string TaskTitle,
    double LeadTimeDays,
    double CycleTimeDays);