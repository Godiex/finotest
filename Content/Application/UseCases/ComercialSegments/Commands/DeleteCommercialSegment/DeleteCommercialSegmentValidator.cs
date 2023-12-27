namespace Application.UseCases.ComercialSegments.Commands.DeleteCommercialSegment;

public class DeleteCommercialSegmentValidator : AbstractValidator<DeleteCommercialSegmentCommand>
{
    public DeleteCommercialSegmentValidator()
    {
        RuleFor(_ => _.Id).NotNull().NotEmpty();
    }
}