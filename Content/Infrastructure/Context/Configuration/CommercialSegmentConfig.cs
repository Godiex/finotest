using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configuration;

public class CommercialSegmentConfig : IEntityTypeConfiguration<CommercialSegment>
{
    public void Configure(EntityTypeBuilder<CommercialSegment> builder)
    {
        builder
            .ToTable("CommercialSegment", SchemaNames.Company);
        
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(70);
        
        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(100);
        
        
        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}
