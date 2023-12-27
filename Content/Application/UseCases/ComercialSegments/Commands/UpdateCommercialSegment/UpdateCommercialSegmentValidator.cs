namespace Application.UseCases.ComercialSegments.Commands.UpdateCommercialSegment;

public class UpdateCommercialSegmentValidator : AbstractValidator<UpdateCommercialSegmentCommand>
{
    public UpdateCommercialSegmentValidator()
    {
        RuleFor(_ => _.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(70);
        RuleFor(_ => _.Description).NotNull().NotEmpty().MinimumLength(3).MaximumLength(100);
    }
}