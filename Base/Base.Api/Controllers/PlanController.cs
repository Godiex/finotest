using Application.UseCases.Plan.Queries;
using Application.UseCases.Users.Commands.LogIn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class PlanController
{
    private readonly IMediator _mediator;

    public PlanController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{userId:guid}/customer")]
    public async Task<PlanByCustomerDto> Post(string userId)
    {
        return new PlanByCustomerDto
        {
            Duration = 30,
            Name = "Convencional",
            Routines = new List<RoutineDto>
            {
                new()
                {
                    Name = "Pierna y Abominales",
                    Training = "Sentadilla,Prensa,Barra libre,Avanzada"
                },
                new()
                {
                    Name = "Pecho y Bicep",
                    Training = "Press de banca,Press superior,Predicador,Martillo"
                },
                new()
                {
                    Name = "Pierna y Abominales",
                    Training = "Sentadilla,Prensa,Barra libre,Avanzada"
                },
                new()
                {
                    Name = "Espalda y Tricep",
                    Training = "Remo,Remo martillo,Copas,Rompe Craneos"
                },
                new()
                {
                    Name = "Circuito",
                    Training = "Bola pared,Power clean,Pull ups"
                },
            }
        };
    }
}
