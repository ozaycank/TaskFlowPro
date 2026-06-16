using TaskFlowPro.Application.Common.Interfaces.Services;

namespace TaskFlowPro.Api.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}