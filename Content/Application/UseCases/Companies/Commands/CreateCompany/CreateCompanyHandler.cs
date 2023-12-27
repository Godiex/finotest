using Domain.Entities;
using Domain.Entities.Idempotency;
using Domain.Entities.ValueObjects;
using Domain.Ports;
using Domain.Services;
using Domain.Services.Companies;

namespace Application.UseCases.Companies.Commands.CreateCompany;

public class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand>
{
    private readonly CompanyCreationService _companyCreationService;
    private readonly IIdempotencyRepository<CompanyId, Guid> _companyIdRepository;
    
    public CreateCompanyHandler(CompanyCreationService service, IIdempotencyRepository<CompanyId, Guid> companyIdReponsitory)
    {
        _companyCreationService = service ?? throw new ArgumentNullException(nameof(service));
        _companyIdRepository = companyIdReponsitory ?? throw new ArgumentNullException(nameof(companyIdReponsitory));
    }

    public async Task<Unit> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        await _companyIdRepository.ValidateEntityId(request.Id);
        await _companyIdRepository.RemoveAsync(request.Id);
        var company = MapCommandToEntity(request);
        await _companyCreationService.CreateAsync(company);
        return new Unit();
    }

    private static Company MapCommandToEntity(CreateCompanyCommand request)
    {
        var identity = new Identity(
            request.AuthorizedAgent.Identity.DocumentType,
            request.AuthorizedAgent.Identity.LegalIdentifier
        );
        var authorizedAgent = new AuthorizedAgent(
            request.AuthorizedAgent.Name,
            request.AuthorizedAgent.Surname,
            request.AuthorizedAgent.Email,
            identity
        );
        var company = new Company(
            request.Id,
            request.Name,
            request.LegalIdentifier,
            request.Hostname,
            authorizedAgent,
            request.CommercialSegment
        );
        return company;
    }
}