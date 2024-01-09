using Application.UseCases.Users.Commands.LogIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<LogInUserDto> Post(LogInUserCommand command) => await _mediator.Send(command);

}
