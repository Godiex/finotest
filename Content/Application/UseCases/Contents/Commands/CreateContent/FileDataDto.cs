namespace Application.UseCases.Contents.Commands.CreateContent;

public record FileDataDto(string Extension, byte[] Data, long Size, string ContentType);