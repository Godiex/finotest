using Domain.Entities;
using Domain.Exceptions.Common;
using Domain.Ports;
using Domain.Services.Companies.Dto;

namespace Domain.Services.Companies;

[DomainService]
public class CompanyUpdateService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly CompanyValidationService _companyValidationService;

    public CompanyUpdateService
        (
        IGenericRepository<Company> companyRepository,
        CompanyValidationService companyValidationService
        )
    {
        _companyRepository = companyRepository;
        _companyValidationService = companyValidationService;
    }

    public async Task UpdateAsync(CompanyToUpdateDto dataToUpdate)
    {
        var company = await FindCompanyById(dataToUpdate.Id);
        await ValidateDataToUpdate(dataToUpdate, company);
        company.Update(
            dataToUpdate.Hostname,
            dataToUpdate.State,
            dataToUpdate.CommercialSegmentId,
            dataToUpdate.AuthorizedAgent
        );
        await _companyRepository.UpdateAsync(company);
    }

    private async Task ValidateDataToUpdate(CompanyToUpdateDto dataToUpdate, Company company)
    {
        await ValidateCommercialSegment(dataToUpdate, company);
        await ValidateAuthorizedAgent(dataToUpdate, company);
        await ValidateHostname(dataToUpdate, company);
    }

    private async Task ValidateCommercialSegment(CompanyToUpdateDto dataToUpdate, Company company)
    {
        if (company.CommercialSegmentId != dataToUpdate.CommercialSegmentId)
        {
            await _companyValidationService.ValidateExistingCommercialSegmentAsync(dataToUpdate.CommercialSegmentId);
        }
    }

    private async Task ValidateAuthorizedAgent(CompanyToUpdateDto dataToUpdate, Company company)
    {
        var authorizedAgent = dataToUpdate.AuthorizedAgent;

        if (company.AuthorizedAgent.Identity.LegalIdentifier != authorizedAgent.Identity.LegalIdentifier)
        {
            await _companyValidationService.ValidateExistingAuthorizedAgentLegalIdentifierAsync(authorizedAgent.Identity.LegalIdentifier);
        }

        if (company.AuthorizedAgent.Identity.DocumentType != authorizedAgent.Identity.DocumentType)
        {
            await _companyValidationService.ValidateExistDocumentTypeAsync(authorizedAgent.Identity.DocumentType);
        }
    }

    private async Task ValidateHostname(CompanyToUpdateDto dataToUpdate, Company company)
    {
        if (company.Hostname != dataToUpdate.Hostname)
        {
            await _companyValidationService.ValidateExistingHostNameAsync(dataToUpdate.Hostname);
        }
    }

    private async Task<Company> FindCompanyById(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        return company ?? throw new ResourceNotFoundException(string.Format(Messages.ResourceNotFoundException, id));
    }
}