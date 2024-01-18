using Domain.Ports;

namespace Domain.Services;

[DomainService]
public class FileStorageService
{
    private readonly IFileStorageRepository _fileStorageRepository;

    public FileStorageService(IFileStorageRepository fileStorageRepository) =>
        _fileStorageRepository = fileStorageRepository;

    public async Task<string> UploadAsync(byte[] fileData, string extencion, string contentType)
    {
        return await _fileStorageRepository.Upload(fileData, extencion, contentType);
    }
}