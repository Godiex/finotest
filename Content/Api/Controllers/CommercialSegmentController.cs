using Application.UseCases.ComercialSegments.Commands.CreateCommercialSegment;
using Application.UseCases.ComercialSegments.Commands.CreateCommercialSegmentId;
using Application.UseCases.ComercialSegments.Commands.DeleteCommercialSegment;
using Application.UseCases.ComercialSegments.Commands.UpdateCommercialSegment;
using Application.UseCases.ComercialSegments.Queries.GetAllCommercialSegment;

namespace Api.Controllers;

[ApiController]
[Route(ApiRoutes.CommercialSegments)]
public class CommercialSegmentController : ControllerBase 
{
    private readonly IMediator _mediator;

    public CommercialSegmentController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<CreateCommercialSegmentIdDto> CreateId()
    {
        return await _mediator.Send(new CreateCommercialSegmentIdCommand());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> CreateCommercialSegment(Guid id, CreateCommercialSegmentCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Accepted();
    }
    
    [HttpPatch("{id:guid}")]
    public async Task Update(Guid id, UpdateCommercialSegmentCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await _mediator.Send(new DeleteCommercialSegmentCommand(id));
    }
    
    [HttpGet]
    public async Task<IEnumerable<GetAllCommercialSegmentDto>> GetAll()
    {
        return await _mediator.Send(new GetAllCommercialSegmentQuery());
    }
}
