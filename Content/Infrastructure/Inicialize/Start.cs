using Infrastructure.Context;
using Infrastructure.Inicialize.Entities;

namespace Infrastructure.Inicialize
{
    public class Start
    {
        private readonly PersistenceContext _context;
        public Start(PersistenceContext context)
        {
            _context = context;
        }

        public async Task InitializeDatabasesAsync()
        {
            await CommercialSegmentSeeder.InitializeAsync(_context);
            await DocumentTypeSeeder.InitializeAsync(_context);
        }
    }
}