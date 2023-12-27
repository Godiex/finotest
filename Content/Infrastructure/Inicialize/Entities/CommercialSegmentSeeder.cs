using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Inicialize.Entities;

public static class CommercialSegmentSeeder
{
    public static async Task InitializeAsync(PersistenceContext context)
    {
        if (context.ComercialSegments.Any()) return;
        var comercialSegments = new List<CommercialSegment>
        {
            new (Guid.NewGuid(), "Seguridad", "Segmento de vigilancia"),
            new (Guid.NewGuid(), "Colchones", "Segmento de Colchones"),
            new (Guid.NewGuid(), "Confecciones", "Segmento de Confecciones"),
            new (Guid.NewGuid(), "Fueza publica", "Segmento de Fueza publica")
        };
        context.AddRange(comercialSegments);
        await context.SaveChangesAsync();
    }
}