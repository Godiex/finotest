using Application.UseCases.Storage.Commands.UploadMultimedia;

namespace Api.Controllers;

[ApiController]
[Route(ApiRoutes.Multimedia)]
public class MultimediaController : ControllerBase 
{
    private readonly IMediator _mediator;

    public MultimediaController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<UploadMultimediaDto> UploadMultimedia([FromForm] UploadMultimediaCommand command)
    {
        return await _mediator.Send(command);
    }
}
