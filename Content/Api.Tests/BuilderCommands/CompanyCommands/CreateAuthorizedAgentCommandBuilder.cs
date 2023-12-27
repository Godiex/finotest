using Application.UseCases.Companies.Commands.CreateCompany;

namespace Api.Tests.BuilderCommands.CompanyCommands
{
    public class CreateAuthorizedAgentCommandBuilder
    {
        private string _name = "Juan";
        private string _surname = "Jimenez";
        private string _email = "test@gmail.com";
        private CreateIdentityCommand _identity = new CreateIdentityCommandBuilder().Build();

        public CreateAuthorizedAgentCommandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CreateAuthorizedAgentCommandBuilder WithSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public CreateAuthorizedAgentCommandBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }
        public CreateAuthorizedAgentCommandBuilder WithIdentity(CreateIdentityCommand identity)
        {
            _identity = identity;
            return this;
        }


        public CreateAuthorizedAgentCommand Build()
        {
            return new CreateAuthorizedAgentCommand(
                _name,
                _surname,
                _email,
                _identity
            );
        }
    }
}
