using Domain.Services;

namespace Application.UseCases.ComercialSegments.Commands.DeleteCommercialSegment;

public class DeleteCommercialSegmentHandler : IRequestHandler<DeleteCommercialSegmentCommand>
{
    private readonly CommercialSegmentService _companyCreationService;
    
    public DeleteCommercialSegmentHandler(CommercialSegmentService service)
    {
        _companyCreationService = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<Unit> Handle(DeleteCommercialSegmentCommand request, CancellationToken cancellationToken)
    {
        await _companyCreationService.DeleteAsync(request.Id);
        return new Unit();
    }
}