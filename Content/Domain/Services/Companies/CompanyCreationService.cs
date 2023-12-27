using Domain.Entities;
using Domain.Entities.Validators;
using Domain.Exceptions.Common;
using Domain.Ports;

namespace Domain.Services.Companies;

[DomainService]
public class CompanyCreationService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly CompanyValidationService _companyValidationService;

    public CompanyCreationService
    (
        IGenericRepository<Company> companyRepository,
        CompanyValidationService companyValidationService
    )
    {
        _companyRepository = companyRepository;
        _companyValidationService = companyValidationService;
    }

    public async Task CreateAsync(Company company)
    {
        await ValidateExistingAuthorizedAgentEmailAsync(company.AuthorizedAgent.Email);
        await ValidateExistingCompanyNameAsync(company.Name);
        await ValidateExistingCompanyLegalIdentifierAsync(company.LegalIdentifier);
        await _companyValidationService.ValidateExistDocumentTypeAsync(company.AuthorizedAgent.Identity.DocumentType);
        await _companyValidationService.ValidateExistingHostNameAsync(company.Hostname);
        await _companyValidationService.ValidateExistingCommercialSegmentAsync(company.CommercialSegmentId);
        await _companyValidationService.ValidateExistingAuthorizedAgentLegalIdentifierAsync(company.AuthorizedAgent.Identity.LegalIdentifier);
        await _companyRepository.AddAsync(company);
    }

    private async Task ValidateExistingCompanyLegalIdentifierAsync(string companyLegalIdentifier)
    {
        bool alredyExistLegalIdentifier = await _companyRepository.Exist(company => company.LegalIdentifier == companyLegalIdentifier);
        if (alredyExistLegalIdentifier)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(companyLegalIdentifier), companyLegalIdentifier);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }

    private async Task ValidateExistingCompanyNameAsync(string name)
    {
        bool alredyExistLegalIdentifier = await _companyRepository.Exist(company => company.Name == name);
        if (alredyExistLegalIdentifier)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(name), name);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }

    private async Task ValidateExistingAuthorizedAgentEmailAsync(string email)
    {
        await EmailValidator.Validate(email);
        bool alredyExistEmail = await _companyRepository.Exist(company => company.AuthorizedAgent.Email == email);
        if (alredyExistEmail)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(email), email);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }
}