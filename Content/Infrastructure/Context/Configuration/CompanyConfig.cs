using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configuration;

public class CompanyConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .ToTable("Company", SchemaNames.Company);

        builder
            .Property(company => company.Name)
            .IsRequired()
            .HasMaxLength(30);
        
        builder
            .Property(company => company.Hostname)
            .IsRequired()
            .HasMaxLength(255);
        
        builder
            .Property(company => company.LegalIdentifier)
            .IsRequired()
            .HasMaxLength(30);
        
        builder
            .Property(company => company.State)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder
            .HasOne(company => company.CommercialSegment)
            .WithMany()
            .HasForeignKey(v => v.CommercialSegmentId);
        
        builder.OwnsOne(company => company.AuthorizedAgent, navigationBuilderAuthorizedAgent =>
        {
            navigationBuilderAuthorizedAgent
                .Property(autorizedAgent => autorizedAgent.Name)
                .IsRequired()
                .HasMaxLength(30);

            navigationBuilderAuthorizedAgent
                .Property(autorizedAgent => autorizedAgent.Surname)
                .IsRequired()
                .HasMaxLength(30);
        
            navigationBuilderAuthorizedAgent
                .Property(autorizedAgent => autorizedAgent.Email)
                .IsRequired()
                .HasMaxLength(255);
            
            navigationBuilderAuthorizedAgent
                .HasIndex(autorizedAgent => autorizedAgent.Email)
                .IsUnique();
            
            navigationBuilderAuthorizedAgent
                .OwnsOne(agent => agent.Identity,
                    navigationBuilderIdentity =>
                    {
                        navigationBuilderIdentity
                            .Property(identity => identity.DocumentType)
                            .IsRequired()
                            .HasMaxLength(4);

                        navigationBuilderIdentity
                            .Property(identity => identity.LegalIdentifier)
                            .IsRequired()
                            .HasMaxLength(15);
                        
                        navigationBuilderIdentity
                            .Property(identity => identity.DocumentType)
                            .IsRequired()
                            .HasMaxLength(4);
                        
                        navigationBuilderIdentity
                            .HasIndex(identity => identity.LegalIdentifier)
                            .IsUnique();
                    });
        });
        
        builder
            .HasIndex(x => x.Name)
            .IsUnique();
        
        builder
            .HasIndex(x => x.LegalIdentifier)
            .IsUnique();
        
        builder
            .HasIndex(x => x.Hostname)
            .IsUnique();
    }
}
