using Domain.Services;

namespace Application.UseCases.ComercialSegments.Commands.UpdateCommercialSegment;

public class UpdateCommercialSegmentHandler : IRequestHandler<UpdateCommercialSegmentCommand>
{
    private readonly CommercialSegmentService _companyCreationService;
    
    public UpdateCommercialSegmentHandler(CommercialSegmentService service)
    {
        _companyCreationService = service ?? throw new ArgumentNullException(nameof(service));
    }

    public async Task<Unit> Handle(UpdateCommercialSegmentCommand request, CancellationToken cancellationToken)
    {
        await _companyCreationService.UpdateAsync(request.Id, request.Name, request.Description);
        return new Unit();
    }
}