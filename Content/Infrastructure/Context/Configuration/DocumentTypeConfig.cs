using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configuration;

public class DocumentTypeConfig : IEntityTypeConfiguration<DocumentType>
{
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        builder
            .ToTable("DocumentType", SchemaNames.Config);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(40);
        
        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(6);
        
        builder
            .HasIndex(x => x.Code)
            .IsUnique();
    }
}
