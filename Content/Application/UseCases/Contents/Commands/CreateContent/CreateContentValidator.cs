namespace Application.UseCases.Contents.Commands.CreateContent;

public class CreateContentValidator : AbstractValidator<CreateContentCommand>
{
    public CreateContentValidator()
    {
        RuleFor(c => c.Tag).NotNull().NotEmpty().MinimumLength(3).MaximumLength(70);
        When(c => c.Languages is not null, () =>
        {
            RuleFor(c => c.Languages).NotEmpty();
        });
        When(c => c.Multimedia is not null, () =>
        {
            RuleFor(c => c.Multimedia).NotEmpty();
        });
        When(c => c.LogoUrl is not null, () =>
        {
            RuleFor(c => c.LogoUrl).NotEmpty().MinimumLength(3).MaximumLength(250);
        });
        When(c => c.TitleContent is not null, () =>
        {
            RuleForEach(c => c.TitleContent).NotEmpty().SetValidator(new TextCommandValidator());
        });
        When(c => c.Items is not null, () =>
        {
            RuleForEach(c => c.Items).NotEmpty().SetValidator(new ItemCommandValidator());
        });
    }
}


public class ItemCommandValidator : AbstractValidator<ItemCommand>
{
    public ItemCommandValidator()
    {
        RuleFor(item => item.Index).NotNull();
        RuleFor(item => item.Behavior).NotNull();
        RuleForEach(item => item.Contents).SetValidator(new SectionContentCommandValidator());
    }
}

public class SectionContentCommandValidator : AbstractValidator<SectionContentCommand>
{
    public SectionContentCommandValidator()
    {
        RuleFor(itemContent => itemContent.Code).NotNull();
        RuleFor(itemContent => itemContent.Label).NotNull();
        RuleFor(itemContent => itemContent.Content).NotNull();
    }
}

public class TextCommandValidator : AbstractValidator<TextCommand>
{
    public TextCommandValidator()
    {
        RuleFor(itemContent => itemContent.Code).NotNull();
        RuleFor(itemContent => itemContent.Content).NotNull();
    }
}