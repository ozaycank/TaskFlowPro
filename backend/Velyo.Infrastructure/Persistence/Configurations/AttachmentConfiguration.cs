using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.StorageKey)
            .IsRequired()
            .HasMaxLength(1000); // S3 key yolları uzun olabilir

        // Tenant Isolation Relationship
        builder.HasOne<Workspace>()
            .WithMany()
            .HasForeignKey(x => x.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Gelecekte Yorumlara (Comments) eklenebilecek dosyalar için
        // Yorum silinirse dosya kaydı da silinir (Cascade)
        builder.HasIndex(x => x.CommentId);
    }
}