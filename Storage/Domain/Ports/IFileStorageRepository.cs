namespace Domain.Ports;

public interface IFileStorageRepository
{
    Task<string> Upload(byte[] fileData, string extencion, string contentType);
    Task Update(string fileUrl, byte[] newFile);
    Task Delete(string fileUrl);
}