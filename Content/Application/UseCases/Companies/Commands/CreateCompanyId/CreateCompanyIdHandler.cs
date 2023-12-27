using Domain.Entities.Idempotency;
using Domain.Ports;

namespace Application.UseCases.Companies.Commands.CreateCompanyId;

public class CreateCompanyIdHandler : IRequestHandler<CreateCompanyIdCommand, CreateCompanyIdDto>
{
    private readonly IIdempotencyRepository<CompanyId, Guid> _repository;

    public CreateCompanyIdHandler(IIdempotencyRepository<CompanyId, Guid> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<CreateCompanyIdDto> Handle(CreateCompanyIdCommand request, CancellationToken cancellationToken)
    {
        var companyId = await _repository.AddAsync(new CompanyId());
        return new CreateCompanyIdDto(companyId.Id);
    }

}