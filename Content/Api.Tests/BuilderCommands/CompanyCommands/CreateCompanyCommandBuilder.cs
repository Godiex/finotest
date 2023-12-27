using Application.UseCases.Companies.Commands.CreateCompany;

namespace Api.Tests.BuilderCommands.CompanyCommands
{
    public class CreateCompanyCommandBuilder
    {

        private string _name = "Togo";
        private string _legalIdentifier = "1234567889";
        private string _hostname = "togo.com";
        private Guid _commercialSegmentId = Guid.NewGuid();
        private CreateAuthorizedAgentCommand _authorizedAgent = new CreateAuthorizedAgentCommandBuilder().Build();

        public CreateCompanyCommandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public CreateCompanyCommandBuilder WithLegalIdentifier(string legalIdentifier)
        {
            _legalIdentifier = legalIdentifier;
            return this;
        }

        public CreateCompanyCommandBuilder WithHostname(string hostname)
        {
            _hostname = hostname;
            return this;
        }
        public CreateCompanyCommandBuilder WithCommercialSegmentId(Guid commercialSegmentId)
        {
            _commercialSegmentId = commercialSegmentId;
            return this;
        }
        public CreateCompanyCommandBuilder WithAuthorizedAgent(CreateAuthorizedAgentCommand authorizedAgent)
        {
            _authorizedAgent = authorizedAgent;
            return this;
        }
        public CreateCompanyCommand Build()
        {
            return new CreateCompanyCommand(
                Guid.NewGuid(),
                _name,
                _legalIdentifier,
                _hostname,
                _authorizedAgent,
                _commercialSegmentId
            );

        }

    }
}
