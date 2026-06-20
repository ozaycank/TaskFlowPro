using Microsoft.EntityFrameworkCore;
using Velyo.Domain.Entities;
using Velyo.Domain.Common.Models;
using Velyo.Domain.Common.Interfaces; // EKSİK OLAN REFERANS EKLENDİ
using Velyo.Infrastructure.Persistence.Interceptors;
using System.Reflection;
using MediatR;
using Newtonsoft.Json;
namespace Velyo.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly AuditableEntityInterceptor _auditableEntityInterceptor;
    private readonly IMediator _mediator;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntityInterceptor auditableEntityInterceptor,
        IMediator mediator)
        : base(options)
    {
        _auditableEntityInterceptor = auditableEntityInterceptor;
        _mediator = mediator;
    }
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<WorkspaceInvitation> WorkspaceInvitations => Set<WorkspaceInvitation>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<DomainEvent>();
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntityInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker.Entries<IHasDomainEvents>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        // Serialize events to OutboxMessages within the SAME transaction
        var outboxMessages = domainEvents.Select(domainEvent =>
            OutboxMessage.Create(
                type: domainEvent.GetType().AssemblyQualifiedName!,
                content: JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            )).ToList();

        OutboxMessages.AddRange(outboxMessages);

        // Single, atomic transaction
        return await base.SaveChangesAsync(cancellationToken);
    }

}