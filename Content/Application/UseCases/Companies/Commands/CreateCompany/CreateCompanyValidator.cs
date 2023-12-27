namespace Application.UseCases.Companies.Commands.CreateCompany;

public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyValidator()
    {
        RuleFor(_ => _.Name).NotNull().NotEmpty().MinimumLength(3).MaximumLength(30);
        RuleFor(_ => _.Hostname).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(_ => _.LegalIdentifier).NotNull().NotEmpty().MinimumLength(5).MaximumLength(30);
        RuleFor(_ => _.AuthorizedAgent.Name).NotNull().MinimumLength(3).MaximumLength(30);
        RuleFor(_ => _.AuthorizedAgent.Surname).NotNull().MinimumLength(3).MaximumLength(30);
        RuleFor(_ => _.AuthorizedAgent.Email).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(_ => _.AuthorizedAgent.Identity.LegalIdentifier).NotNull().NotEmpty().MinimumLength(5).MaximumLength(15);
        RuleFor(_ => _.AuthorizedAgent.Identity.DocumentType).NotNull().NotEmpty().MinimumLength(1).MaximumLength(4);
    }
}