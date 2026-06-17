namespace Velyo.Application.Common.Interfaces.Services;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}