using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Inicialize.Entities;

public static class DocumentTypeSeeder
{
    public static async Task InitializeAsync(PersistenceContext context)
    {
        if (context.DocumentTypes.Any()) return;
        var documentTypes = new List<DocumentType>
        {
            new ("CC", "Cédula de Ciudadanía"),
            new ("TI", "Tarjeta de Identidad"),
            new ("PP", "Pasaporte"),
            new ("TP", "Tarjeta Profesional")
        };
        context.AddRange(documentTypes);
        await context.SaveChangesAsync();
    }
}