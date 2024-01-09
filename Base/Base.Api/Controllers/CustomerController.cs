using Application.UseCases.Customers.Queries;
using Application.UseCases.Customers.Commands.Create;
using Application.UseCases.Customers.Commands.Delete;
using Application.UseCases.Customers.Commands.Update;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CustomerController
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{gymId:guid}/gym")]
    public async Task<List<CustomerDto>> Get(Guid gymId) => await _mediator.Send(new CustomerQuery(gymId));

    [HttpPost]
    public async Task Post(CustomerCreateCommand command) => await _mediator.Send(command);

    [HttpPatch("{customerId:guid}")]
    public async Task Patch(CustomerUpdateCommand command, Guid customerId)
    {
        if (customerId != command.Id)
        {
            throw new AppException("Los Id no coinciden");
        }
        await _mediator.Send(command);
    }

    [HttpDelete]
    public async Task Delete(CustomerDeleteCommand command) => await _mediator.Send(command);
}
