using Domain.Entities;
using Domain.Entities.Base;
using Domain.Entities.Idempotency;
using Infrastructure.Extensions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Context
{
    public class PersistenceContext : DbContext
    {
        private readonly DatabaseSettings _databaseSettings;

        public PersistenceContext(
            DbContextOptions<PersistenceContext> options,
            IOptions<DatabaseSettings> databaseSettings
        ) : base(options)
        {
            _databaseSettings = databaseSettings.Value ?? throw new ArgumentNullException(nameof(databaseSettings.Value));
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<CommercialSegment> ComercialSegments { get; set; }
        public DbSet<CompanyId> CompanyIds { get; set; }
        public DbSet<CommercialSegmentId> CommercialSegmentIds { get; set; }


        public async Task CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        protected override void OnModelCreating(ModelBuilder? modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            modelBuilder.HasDefaultSchema(_databaseSettings.SchemaName);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var t = entityType.ClrType;
                if (!typeof(DomainEntity).IsAssignableFrom(t)) continue;
                modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn").HasDefaultValueSql("GETDATE()");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModifiedOn").HasDefaultValueSql("GETDATE()");
            }
            modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
