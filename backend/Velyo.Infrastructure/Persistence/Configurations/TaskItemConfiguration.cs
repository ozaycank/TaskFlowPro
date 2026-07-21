using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using Velyo.Domain.Entities;
using System;
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

        builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CustomFieldsData)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                   v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null) ?? new Dictionary<string, string>())
               .HasColumnType("jsonb")
               .IsRequired();

        builder.HasOne<Sprint>()
                .WithMany()
                .HasForeignKey(x => x.SprintId)
                .OnDelete(DeleteBehavior.SetNull);

        // Kendi kendine ilişki (Self-Referencing): Bir alt görev (Sub-task), ana göreve (ParentTask) bağlanır.
        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(x => x.ParentTaskId)
            .OnDelete(DeleteBehavior.Restrict); // Ana görev silinirse alt görevleri otomatik silmeyelim, ya da null yapalım (İhtiyaca göre Cascade yapılabilir)

        // Etiketler listesi veritabanına direkt kaydedilir (Postgres için text[])
        builder.Property(x => x.Labels)
             .HasColumnType("text[]")
             .HasDefaultValueSql("'{}'");
    }
}