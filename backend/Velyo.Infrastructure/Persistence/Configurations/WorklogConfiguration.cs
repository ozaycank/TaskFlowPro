using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Configurations;

public class WorklogConfiguration : IEntityTypeConfiguration<Worklog>
{
    public void Configure(EntityTypeBuilder<Worklog> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TimeSpentSeconds)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        // Foreign Key Relationships
        builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade); // If Workspace is deleted, delete worklogs

        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Cascade); // If Task is deleted, delete worklogs

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Keep logs even if user is deactivated/deleted (or Cascade based on your business rule)
    }
}