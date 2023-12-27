namespace Application.UseCases.ComercialSegments.Commands.CreateCommercialSegment;

public class CreateCommercialSegmentValidator : AbstractValidator<CreateCommercialSegmentCommand>
{
    public CreateCommercialSegmentValidator()
    {
        RuleFor(_ => _.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(30);
    }
}