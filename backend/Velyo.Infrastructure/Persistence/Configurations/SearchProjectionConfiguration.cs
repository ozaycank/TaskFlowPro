using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using Velyo.Domain.Entities;

namespace Velyo.Infrastructure.Persistence.Configurations;

public class SearchProjectionConfiguration : IEntityTypeConfiguration<SearchProjection>
{
    public void Configure(EntityTypeBuilder<SearchProjection> builder)
    {
        builder.HasKey(x => x.Id);

        // Domain'i temiz tutmak için Shadow Property kullanımına geri dönüyoruz.
        builder.Property<NpgsqlTsVector>("SearchVector")
            .HasComputedColumnSql("to_tsvector('english'::regconfig, coalesce(\"Title\", '') || ' ' || coalesce(\"Content\", ''))", stored: true);

        // Apply the GIN index targeting the shadow property
        builder.HasIndex("SearchVector")
            .HasMethod("GIN");

        builder.HasIndex(x => x.WorkspaceId);
    }
}