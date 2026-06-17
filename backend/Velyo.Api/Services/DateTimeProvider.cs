using Velyo.Application.Common.Interfaces.Services;

namespace Velyo.Api.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}