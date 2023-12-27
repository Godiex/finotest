using Application.UseCases.Companies.Commands.CreateCompany;
using Application.UseCases.Companies.Commands.CreateCompanyId;
using Application.UseCases.Companies.Commands.UpdateCompany;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/companies")]
public class CompanyController : ControllerBase 
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<CreateCompanyIdDto> CreateId()
    {
        return await _mediator.Send(new CreateCompanyIdCommand());
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> CreateCompany(Guid id, CreateCompanyCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return Accepted();
    }
    
    [HttpPatch("{id:guid}")]
    public async Task UpdateCompany(Guid id, [FromBody]UpdateCompanyCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
    }
}
