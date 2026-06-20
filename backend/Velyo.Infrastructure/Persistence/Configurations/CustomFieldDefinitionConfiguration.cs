using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Configurations;

public class CustomFieldDefinitionConfiguration : IEntityTypeConfiguration<CustomFieldDefinition>
{
    public void Configure(EntityTypeBuilder<CustomFieldDefinition> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.OptionsJson).HasColumnType("jsonb"); // If options exist, store as JSONB

        builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Complex index: A project cannot have two fields with the same name
        builder.HasIndex(x => new { x.WorkspaceId, x.ProjectId, x.Name }).IsUnique();
    }
}