using Microsoft.EntityFrameworkCore;
using TaskFlowPro.Domain.Entities;
using TaskFlowPro.Domain.Common.Models; // DomainEvent'i tanıyabilmesi için ekledik
using TaskFlowPro.Infrastructure.Persistence.Interceptors;
using System.Reflection;

namespace TaskFlowPro.Infrastructure.Persistence;

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // FIXED: EF Core'a DomainEvent sınıfını veritabanı modeli olarak taramamasını söylüyoruz.
        builder.Ignore<DomainEvent>();

        // Assemblies içindeki IEntityTypeConfiguration'ları yükler
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntityInterceptor);
    }
}