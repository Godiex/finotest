using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
    
    public async Task<string> Upload(byte[] fileData, string extencion, string contentType)
    {
        BlobContainerClient containerClient = new(_storageSettings.ConnectionString, _storageSettings.Container);
        var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid()}{extencion}");
        using var fileStream = new MemoryStream(fileData);
        BlobUploadOptions blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
        };
        await blobClient.UploadAsync(fileStream, blobUploadOptions).ConfigureAwait(false);
        fileStream.Close();
        return blobClient.Uri.ToString();
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