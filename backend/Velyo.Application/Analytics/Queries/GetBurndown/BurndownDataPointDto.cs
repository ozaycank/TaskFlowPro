namespace Velyo.Application.Analytics.Queries.GetBurndown;

public record BurndownDataPointDto(
    DateTimeOffset Date,
    int RemainingTasks,
    decimal IdealTasks);