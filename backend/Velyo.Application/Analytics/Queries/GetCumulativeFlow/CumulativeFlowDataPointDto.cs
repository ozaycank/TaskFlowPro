namespace Velyo.Application.Analytics.Queries.GetCumulativeFlow;

public record CumulativeFlowDataPointDto(
    DateTimeOffset Date,
    string StateName,
    int TaskCount);