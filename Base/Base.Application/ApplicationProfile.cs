using Application.UseCases.Voters.Queries.GetVoter;
using AutoMapper;
using Domain.Entities;

namespace Application
{

    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Voter, VoterDto>().ReverseMap();
        }
    }
}