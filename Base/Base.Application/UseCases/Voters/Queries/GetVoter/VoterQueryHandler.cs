using Application.UseCases.Voters.Queries.GetVoter;
using AutoMapper;
using Domain.Entities;
using Domain.Ports;
using MediatR;

namespace Application.Voters;

public class VoterQueryHandler : IRequestHandler<VoterQuery, VoterDto>
{
    private readonly IGenericRepository<Voter> _repository;
    private readonly IMapper _mapper;

    public VoterQueryHandler(IGenericRepository<Voter> repository, IMapper mapper) =>
        (_repository, _mapper) = (repository, mapper);


    public async Task<VoterDto> Handle(VoterQuery request, CancellationToken cancellationToken)
    {
        var voter = (await _repository.GetAsync()).ToList();
        return _mapper.Map<VoterDto>(voter);
    }
}