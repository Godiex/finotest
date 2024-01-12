using Azure.Storage.Blobs;
using Domain.Ports;
using Infrastructure.Extensions.Storage;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Adapters.Repository;

public class FileStorageRepository : IFileStorageRepository
{
    private readonly StorageSettings _storageSettings;
    
    public FileStorageRepository(IConfiguration configuration)
    {
        _storageSettings = configuration.GetSection(nameof(StorageSettings)).Get<StorageSettings>();
    }
    
    public async Task<string> Upload(byte[] fileData, string fileName)
    {
        var a = GetFileExtension(fileData);
        BlobContainerClient containerClient = new(_storageSettings.ConnectionString, _storageSettings.Container);
        var blobClient = containerClient.GetBlobClient(fileName);
        using var fileStream = new MemoryStream(fileData);
        await blobClient.UploadAsync(fileStream, true).ConfigureAwait(false);
        fileStream.Close();
        return blobClient.Uri.ToString();
    }
    
    public string GetFileExtension(byte[] fileBytes)
    {
        var provider = new FileExtensionContentTypeProvider();
        string fileExtension;

        // Proporcionar un nombre de archivo ficticio
        var fileName = "tempfile";
        
        // Intentar obtener la extensión a partir del contenido del archivo
        if (provider.TryGetContentType(fileName, out string contentType))
        {
            provider.Mappings.TryGetValue(contentType, out fileExtension);
        }
        else
        {
            fileExtension = ".bin"; // Extensión predeterminada si no se puede determinar
        }

        return fileExtension;
    }

    public Task Update(string fileUrl, byte[] newFile)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string fileUrl)
    {
        throw new NotImplementedException();
    }
}