namespace Application.UseCases.Contents.Commands.CreateContent;

public class CreateContentValidator : AbstractValidator<CreateContentEntryCommand>
{
    public CreateContentValidator()
    {
        RuleFor(c => c.Tag).NotNull().NotEmpty().MinimumLength(3).MaximumLength(70);
        When(c => c.Languages is not null, () =>
        {
            RuleFor(c => c.Languages).NotEmpty();
        });
        When(c => c.Carousel is not null, () =>
        {
            RuleFor(c => c.Carousel).NotEmpty();
        });
        When(c => c.Contents is not null, () =>
        {
            RuleForEach(c => c.Contents).NotEmpty();
        });
    }
}


public class ItemCommandValidator : AbstractValidator<AccordionDetailCommand>
{
    public ItemCommandValidator()
    {
        RuleFor(item => item.Index).NotNull();
        RuleFor(item => item.Behavior).NotNull();
        RuleForEach(item => item.Data).NotNull();
    }
}

public class DetailCommandValidator : AbstractValidator<AccordionDetailCommand>
{
    public DetailCommandValidator()
    {
        RuleFor(itemContent => itemContent.Behavior).NotNull();
        RuleFor(itemContent => itemContent.Data).NotNull();
        When(c => c.Label is not null, () =>
        {
            RuleFor(c => c.Label).NotEmpty();
        });
    }
}
