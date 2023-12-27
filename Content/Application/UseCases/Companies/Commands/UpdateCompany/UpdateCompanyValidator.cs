using Domain.Enums;

namespace Application.UseCases.Companies.Commands.UpdateCompany;

public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyValidator()
    {
        RuleFor(_ => _.Hostname).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(_ => _.Hostname).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(_ => _.AuthorizedAgent.Name).NotNull().MinimumLength(3).MaximumLength(30);
        RuleFor(_ => _.AuthorizedAgent.Surname).NotNull().MinimumLength(3).MaximumLength(30);
        RuleFor(_ => _.AuthorizedAgent.Email).NotNull().NotEmpty().MinimumLength(5).MaximumLength(255);
        RuleFor(_ => _.AuthorizedAgent.Identity.LegalIdentifier).NotNull().NotEmpty().MinimumLength(5).MaximumLength(15);
        RuleFor(_ => _.AuthorizedAgent.Identity.DocumentType).NotNull().NotEmpty().MinimumLength(1).MaximumLength(4);
        RuleFor(_ => _.State).NotNull().Must(state => state != CompanyState.DELETED);
    }
}