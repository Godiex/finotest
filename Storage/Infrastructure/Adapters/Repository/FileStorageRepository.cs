using Azure.Storage.Blobs;
using Domain.Ports;
using Infrastructure.Extensions.Storage;
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
        BlobContainerClient containerClient = new(_storageSettings.ConnectionString, _storageSettings.Container);
        var blobClient = containerClient.GetBlobClient(fileName);
        using var fileStream = new MemoryStream(fileData);
        await blobClient.UploadAsync(fileStream, true).ConfigureAwait(false);
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