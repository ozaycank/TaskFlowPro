using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Title).HasMaxLength(300).IsRequired();
        builder.Property(d => d.Content).HasColumnType("text").IsRequired(); // Unbounded text for Markdown
        builder.Property(d => d.EmojiIcon).HasMaxLength(10);

        builder.HasOne<Workspace>().WithMany().HasForeignKey(d => d.WorkspaceId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Project>().WithMany().HasForeignKey(d => d.ProjectId).OnDelete(DeleteBehavior.Cascade);

        // Self-referencing relationship for Document Hierarchy
        builder.HasOne<Document>().WithMany().HasForeignKey(d => d.ParentDocumentId).OnDelete(DeleteBehavior.Restrict);
    }
}