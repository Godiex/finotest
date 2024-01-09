using Application.UseCases.GymOwners.Commands.Create;
using Application.UseCases.GymOwners.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Base.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class GymOwnerController
{
    private readonly IMediator _mediator;

    public GymOwnerController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{userId:guid}/user")]
    public async Task<GymOwnerDto> Get(Guid userId) => await _mediator.Send(new GymOwnerQuery(userId));

    [HttpPost]
    public async Task Post(GymOwnerCreateCommand command) => await _mediator.Send(command);

}
