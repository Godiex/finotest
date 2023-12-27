using Domain.Services.Companies;
using Domain.Services.Companies.Dto;

namespace Application.UseCases.Companies.Commands.UpdateCompany;

public class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly CompanyUpdateService _companyUpdateService;

    public UpdateCompanyHandler(CompanyUpdateService companyUpdateService)
    {
        _companyUpdateService = companyUpdateService ?? throw new ArgumentNullException(nameof(companyUpdateService));
    }

    public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var agentToUpdateDto = new AuthorizeAgentToUpdateDto(
            request.AuthorizedAgent.Name,
            request.AuthorizedAgent.Surname,
            request.AuthorizedAgent.Email,
            new IdentityToUpdateDto(
                request.AuthorizedAgent.Identity.DocumentType,
                request.AuthorizedAgent.Identity.LegalIdentifier
            )
        );
        var dataToUpdate = new CompanyToUpdateDto(
            request.Id,
            request.Hostname,
            request.State,
            request.CommercialSegment,
            agentToUpdateDto
        );
        await _companyUpdateService.UpdateAsync(dataToUpdate);
        return new Unit();
    }
}