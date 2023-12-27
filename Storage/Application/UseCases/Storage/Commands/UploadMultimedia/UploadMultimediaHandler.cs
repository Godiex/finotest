using Domain;
using Domain.Exceptions.Common;
using Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Storage.Commands.UploadMultimedia
{
    public class UploadMultimediaHandler : IRequestHandler<UploadMultimediaCommand, UploadMultimediaDto>
    {
        private readonly FileStorageService _fileStorageService;
        private const long MaxFileSize = 20 * 1024 * 1024;

        public UploadMultimediaHandler(FileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
        }

        public async Task<UploadMultimediaDto> Handle(UploadMultimediaCommand request, CancellationToken cancellationToken)
        {
            foreach (var file in request.Files)
            {
                if (file.Length <= MaxFileSize) continue;
                var message = string.Format(Messages.InvalidFileSizeException, file.FileName,
                    MaxFileSize / (1024 * 1024));
                throw new InvalidFileSizeException(message);
            }
            
            var multimediaUrls = await Task.WhenAll(request.Files.Select(file =>
                ProcessFileAsync(file, cancellationToken)));

            return new UploadMultimediaDto(multimediaUrls.ToList());
        }

        private async Task<string> ProcessFileAsync(IFormFile file, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var extension = GetFileExtension(file);
            var fileName = GenerateUniqueFileNameWithExtension(extension);
            
            return await _fileStorageService.UploadAsync(memoryStream.ToArray(), fileName);
        }

        private static string GetFileExtension(IFormFile file)
        {
            return Path.GetExtension(file.FileName);
        }

        private static string GenerateUniqueFileNameWithExtension(string extension)
        {
            return $"{Guid.NewGuid()}{extension}";
        }
    }
}