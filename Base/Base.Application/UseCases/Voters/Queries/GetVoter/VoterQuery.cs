
using MediatR;

namespace Application.UseCases.Voters.Queries.GetVoter;

public record VoterQuery(
    Guid uid
    ) : IRequest<VoterDto>;
