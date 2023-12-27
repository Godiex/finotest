using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Storage.Commands.UploadMultimedia;

public record UploadMultimediaCommand(IEnumerable<IFormFile> Files) : IRequest<UploadMultimediaDto>;
