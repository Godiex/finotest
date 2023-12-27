using Domain.Entities;
using Domain.Entities.Validators;
using Domain.Exceptions.Common;
using Domain.Ports;

namespace Domain.Services.Companies;

[DomainService]
public class CompanyValidationService
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IGenericRepository<CommercialSegment> _commercialSegmentRepository;
    private readonly IGenericRepository<DocumentType> _documentTypeRepository;

    public CompanyValidationService(

        IGenericRepository<Company> companyRepository,
        IGenericRepository<CommercialSegment> commercialSegmentRepository,
        IGenericRepository<DocumentType> documentTypeRepository
    )
    {
        _companyRepository = companyRepository;
        _commercialSegmentRepository = commercialSegmentRepository;
        _documentTypeRepository = documentTypeRepository;
    }

    public async Task ValidateExistingHostNameAsync(string host)
    {
        await HostValidator.Validate(host);
        bool alredyExistHost = await _companyRepository.Exist(company => company.Hostname == host);
        if (alredyExistHost)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(host), host);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }

    public async Task ValidateExistingCommercialSegmentAsync(Guid id)
    {
        bool existCommercialSegment = await _commercialSegmentRepository.Exist(commercialSegment => commercialSegment.Id == id);
        if (!existCommercialSegment)
        {
            string exceptionMessage = string.Format(Messages.ResourceNotFoundException, id);
            throw new ResourceNotFoundException(exceptionMessage);
        }
    }

    public async Task ValidateExistDocumentTypeAsync(string codeDocumentType)
    {
        bool existDocumentType = await _documentTypeRepository.Exist(documentType => documentType.Code == codeDocumentType);
        if (!existDocumentType)
        {
            string exceptionMessage = string.Format(Messages.ResourceNotFoundException, codeDocumentType);
            throw new ResourceNotFoundException(exceptionMessage);
        }
    }

    public async Task ValidateExistingAuthorizedAgentLegalIdentifierAsync(string authorizedAgentLegalIdentifier)
    {
        bool alredyExistLegalIdentifier = await _companyRepository.Exist(company => company.AuthorizedAgent.Identity.LegalIdentifier == authorizedAgentLegalIdentifier);
        if (alredyExistLegalIdentifier)
        {
            string exceptionMessage = string.Format(Messages.AlredyExistException, nameof(authorizedAgentLegalIdentifier), authorizedAgentLegalIdentifier);
            throw new ResourceAlreadyExistException(exceptionMessage);
        }
    }
}