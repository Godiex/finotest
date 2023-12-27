using Application.UseCases.Companies.Commands.UpdateCompany;
using Domain.Enums;

namespace Api.Tests.BuilderCommands.CompanyCommands
{
    public class UpdateCompanyCommandBuilder
    {

    
        
        private string _hostname = "togo.com";
        private Guid _commercialSegmentId = Guid.NewGuid();
        private CompanyState _state = CompanyState.ENABLE;
        private UpdateAuthorizeAgentCommand _authorizedAgent = new UpdateAuthorizedAgentCommandBuilder().Build();

     
        public UpdateCompanyCommandBuilder WithHostname(string hostname)
        {
            _hostname = hostname;
            return this;
        }
        public UpdateCompanyCommandBuilder WithCommercialSegmentId(Guid commercialSegmentId)
        {
            _commercialSegmentId = commercialSegmentId;
            return this;
        }
        public UpdateCompanyCommandBuilder WithAuthorizedAgent(UpdateAuthorizeAgentCommand authorizedAgent)
        {
            _authorizedAgent = authorizedAgent;
            return this;
        }
        public UpdateCompanyCommand Build()
        {
            return new UpdateCompanyCommand(
                Guid.NewGuid(),
                _hostname,
                _commercialSegmentId,
                _state,
                _authorizedAgent
            );

        }

    }
}
