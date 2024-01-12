using Application.Common.Helpers.Pagination;
using Application.UseCases.Contents.Commands.CreateContent;
using Application.UseCases.Contents.Commands.CreateContentId;
using Application.UseCases.Contents.Queries.GetAllContentsPaginated;

namespace Api.Controllers;

[ApiController]
[Route(ApiRoutes.Content)]
public class ContentController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContentController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<CreateContentIdDto> CreateId()
    {
        return await _mediator.Send(new CreateContentIdCommand());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> CreateContent(Guid id, [FromForm] CreateContentEntryCommand command)
    {
        await _mediator.Send(new CreateContentCommand(id, command));
        return Accepted();
    }

    [HttpGet]
    public async Task<PaginationResponse<GetAllContentsPaginatedDto>> CreateContent(int pageNumber = 1, int pageSize = 5, string filter = "")
    {
        return await _mediator.Send(new GetAllContentsPaginatedQuery(filter, pageNumber, pageSize));
    }

}
