namespace Application.UseCases.Storage.Commands.UploadMultimedia;

public record DataMessageDto(string Subscription, IEnumerable<FileDataDto> Files);
public record FileDataDto(string Extension, byte[] Data, long Size);