using Velyo.Domain.Entities;

namespace Velyo.Application.Common.Interfaces.Repositories;

public interface IActivityLogRepository
{
    void Add(ActivityLog log);
}