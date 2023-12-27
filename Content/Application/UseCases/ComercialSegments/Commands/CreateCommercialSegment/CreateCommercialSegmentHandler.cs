using Domain.Entities;
using Domain.Entities.Idempotency;
using Domain.Ports;
using Domain.Services;

namespace Application.UseCases.ComercialSegments.Commands.CreateCommercialSegment;

public class CreateCommercialSegmentHandler : IRequestHandler<CreateCommercialSegmentCommand>
{
    private readonly CommercialSegmentService _companyCreationService;
    private readonly IIdempotencyRepository<CommercialSegmentId, Guid> _commercialSegmentIdRepository;
    
    public CreateCommercialSegmentHandler(CommercialSegmentService service, IIdempotencyRepository<CommercialSegmentId, Guid> commercialSegmentIdRepository)
    {
        _companyCreationService = service ?? throw new ArgumentNullException(nameof(service));
        _commercialSegmentIdRepository = commercialSegmentIdRepository ?? throw new ArgumentNullException(nameof(commercialSegmentIdRepository));
    }

    public async Task<Unit> Handle(CreateCommercialSegmentCommand request, CancellationToken cancellationToken)
    {
        await _commercialSegmentIdRepository.ValidateEntityId(request.Id);
        await _commercialSegmentIdRepository.RemoveAsync(request.Id);
        var commercialSegment = new CommercialSegment(request.Id, request.Name, request.Description);
        await _companyCreationService.CreateAsync(commercialSegment);
        return new Unit();
    }
}