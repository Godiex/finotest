namespace Application.UseCases.Contents.Commands.DeleteContent;

public class DeleteContentValidator : AbstractValidator<DeleteContentCommand>
{
    public DeleteContentValidator()
    {
        RuleFor(_ => _.Id).NotNull().NotEmpty();
    }
}