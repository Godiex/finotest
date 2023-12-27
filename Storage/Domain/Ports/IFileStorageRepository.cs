namespace Domain.Ports;

public interface IFileStorageRepository
{
    Task<string> Upload(byte[] fileData, string fileName);
    Task Update(string fileUrl, byte[] newFile);
    Task Delete(string fileUrl);
}