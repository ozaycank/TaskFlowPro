using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Configurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.EntityType).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Action).HasMaxLength(100).IsRequired();
        builder.Property(a => a.Details).HasColumnType("text");

        // Fast lookups for feeds
        builder.HasIndex(a => a.WorkspaceId);
        builder.HasIndex(a => a.ProjectId);
        builder.HasIndex(a => a.TaskId);
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.CreatedAt);

        // No hard foreign keys for soft-deleted entities to avoid breaking logs when tasks are deleted
    }
}