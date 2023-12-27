using Application.UseCases.Companies.Commands.UpdateCompany;

namespace Api.Tests.BuilderCommands.CompanyCommands
{
    public class UpdateAuthorizedAgentCommandBuilder
    {
        private string _name = "Juan";
        private string _surname = "Jimenez";
        private string _email = "test@gmail.com";
        private UpdateIdentityCommand _identity = new UpdateIdentityCommandBuilder().Build();

        public UpdateAuthorizedAgentCommandBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UpdateAuthorizedAgentCommandBuilder WithSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public UpdateAuthorizedAgentCommandBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }
        public UpdateAuthorizedAgentCommandBuilder WithIdentity(UpdateIdentityCommand identity)
        {
            _identity = identity;
            return this;
        }


        public UpdateAuthorizeAgentCommand Build()
        {
            return new UpdateAuthorizeAgentCommand(
                _name,
                _surname,
                _email,
                _identity
            );
        }
    }
}
