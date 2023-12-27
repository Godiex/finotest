namespace Application.UseCases.Storage.Commands.UploadMultimedia;

public class UploadMultimediaValidator : AbstractValidator<UploadMultimediaCommand>
{
    public UploadMultimediaValidator()
    {
        RuleFor(_ => _.Files).NotNull().NotEmpty();
    }
}