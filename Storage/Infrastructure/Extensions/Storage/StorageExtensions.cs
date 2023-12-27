using Domain.Ports;
using Infrastructure.Adapters.Repository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Storage;

public static class StorageExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<StorageSettings>(config.GetSection(nameof(StorageSettings)));
        services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = long.MaxValue; // Ajusta según tus necesidades
            options.MemoryBufferThreshold = int.MaxValue; // Puedes ajustar este valor según tus necesidades
        });

        services.AddSingleton(typeof(IFileStorageRepository), typeof(FileStorageRepository));
        return services;
    }
}