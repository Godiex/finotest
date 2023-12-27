using Domain.Entities.Idempotency;
using Domain.Ports;

namespace Application.UseCases.ComercialSegments.Commands.CreateCommercialSegmentId;

public class CreateCommercialSegmentIdHandler : IRequestHandler<CreateCommercialSegmentIdCommand, CreateCommercialSegmentIdDto>
{
    private readonly IIdempotencyRepository<CommercialSegmentId, Guid> _repository;

    public CreateCommercialSegmentIdHandler(IIdempotencyRepository<CommercialSegmentId, Guid> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<CreateCommercialSegmentIdDto> Handle(CreateCommercialSegmentIdCommand request, CancellationToken cancellationToken)
    {
        var CommercialSegmentId = await _repository.AddAsync(new CommercialSegmentId());
        return new CreateCommercialSegmentIdDto(CommercialSegmentId.Id);
    }

}