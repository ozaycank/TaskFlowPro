using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Velyo.Application.Common.Interfaces.Data;
using Velyo.Application.Common.Interfaces.Repositories;
using Velyo.Application.Common.Interfaces.Services;
using Velyo.Infrastructure.Persistence;
using Velyo.Infrastructure.Persistence.Interceptors;
using Velyo.Infrastructure.Persistence.Repositories;
using Velyo.Infrastructure.BackgroundJobs;
using Velyo.Infrastructure.Services;
using Velyo.Infrastructure.Storage;
using Velyo.Infrastructure.RealTime.Presence;

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
        services.AddScoped<IWorkspaceInvitationRepository, WorkspaceInvitationRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICustomFieldDefinitionRepository, CustomFieldDefinitionRepository>();
        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
        services.AddScoped<ISprintRepository, SprintRepository>();
        services.AddScoped<ISearchProjectionRepository, SearchProjectionRepository>();
        services.AddSignalR();
        services.AddScoped<IRealTimeNotifier, Velyo.Infrastructure.RealTime.SignalRNotifier>();
        services.AddSingleton<IPresenceTracker, PresenceTracker>();
        services.AddHostedService<OutboxProcessorBackgroundService>();
        services.AddTransient<IEmailService, MockEmailService>();
        services.AddScoped<IStorageService, MockStorageService>();
        services.AddScoped<Velyo.Application.Common.Interfaces.Services.ISearchService, Velyo.Infrastructure.Search.PostgresFullTextSearchService>();
        Stripe.StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
        services.AddScoped<Velyo.Application.Common.Interfaces.Services.IBillingService, Velyo.Infrastructure.Billing.StripeBillingService>();
        services.AddScoped<IWorklogRepository, WorklogRepository>();
        return services;
    }
}