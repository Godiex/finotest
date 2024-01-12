using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Storage.Commands.UploadMultimedia;

public record UploadMultimediaCommand(DataMessageDto FilesMetadata) : IRequest<UploadMultimediaDto>;

public record UploadMultimediaEntryCommand
{
    public UploadMultimediaEntryCommand()
    {
    }

    public UploadMultimediaEntryCommand(IEnumerable<IFormFile> files)
    {
        Files = files;
    }
    
    public DataMessageDto GetDataMessageDto()
    {
        IEnumerable<FileDataDto> fileDataDtos = Files.Select(ConvertToFileDataDto).ToArray();
        string queue = Guid.NewGuid().ToString();
        return new DataMessageDto(queue, fileDataDtos);
    }

    private static FileDataDto ConvertToFileDataDto(IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        string extension = Path.GetExtension(formFile.FileName);
        byte[] data = memoryStream.ToArray();
        return new FileDataDto(extension, data, formFile.Length);
    }

    public IEnumerable<IFormFile> Files { get; set; }
    
}
