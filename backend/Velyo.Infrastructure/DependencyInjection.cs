using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Infrastructure.Persistence;
using Velyo.Infrastructure.Persistence.Interceptors;
using Velyo.Infrastructure.Persistence.Repositories;

namespace Velyo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        services.AddScoped<IWorkspaceMemberRepository, WorkspaceMemberRepository>();

        // Note: ICurrentUserService and IDateTimeProvider are not registered here.
        // ICurrentUserService depends on HttpContext (API Layer).
        // IDateTimeProvider can be registered here or in Application Layer, but usually API/Infrastructure shared.

        return services;
    }
}