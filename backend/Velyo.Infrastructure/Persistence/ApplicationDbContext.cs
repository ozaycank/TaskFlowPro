using Microsoft.EntityFrameworkCore;
using Velyo.Domain.Entities;
using Velyo.Domain.Common.Models; // DomainEvent'i tanÄ±yabilmesi iÃ§in ekledik
using Velyo.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace Velyo.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly AuditableEntityInterceptor _auditableEntityInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditableEntityInterceptor auditableEntityInterceptor)
        : base(options)
    {
        _auditableEntityInterceptor = auditableEntityInterceptor;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();
    public DbSet<WorkspaceMember> WorkspaceMembers => Set<WorkspaceMember>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<WorkspaceInvitation> WorkspaceInvitations => Set<WorkspaceInvitation>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // FIXED: EF Core'a DomainEvent sÄ±nÄ±fÄ±nÄ± veritabanÄ± modeli olarak taramamasÄ±nÄ± sÃ¶ylÃ¼yoruz.
        builder.Ignore<DomainEvent>();

        // Assemblies iÃ§indeki IEntityTypeConfiguration'larÄ± yÃ¼kler
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntityInterceptor);
    }
}