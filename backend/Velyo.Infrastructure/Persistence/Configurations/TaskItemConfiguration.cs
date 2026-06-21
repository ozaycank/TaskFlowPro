using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);

        // Explicit link to Workspace for Multi-tenancy filtering
        builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
        // This explicitly tells EF Core Npgsql to map the Dictionary to a JSONB column
        builder.Property(x => x.CustomFieldsData)
               .HasColumnType("jsonb")
               .IsRequired();
        builder.HasOne<Sprint>()
                .WithMany()
                .HasForeignKey(x => x.SprintId)
                .OnDelete(DeleteBehavior.SetNull); // Sprint silinirse tasklar Backlog'a (null) düşer
    }
}