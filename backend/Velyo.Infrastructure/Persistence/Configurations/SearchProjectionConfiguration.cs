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

        // 1. Explicitly declare the Shadow Property on the entity
        builder.Property<NpgsqlTsVector>("SearchVector")
            // FIXED: Replaced 'concat_ws' (STABLE) with 'COALESCE' and '||' (IMMUTABLE)
            // PostgreSQL mandates strict immutability for STORED generated columns.
            .HasComputedColumnSql("to_tsvector('english'::regconfig, coalesce(\"Title\", '') || ' ' || coalesce(\"Content\", ''))", stored: true);

        // 2. Apply the GIN index targeting the shadow property
        builder.HasIndex("SearchVector")
            .HasMethod("GIN");

        builder.HasIndex(x => x.WorkspaceId);
    }
}