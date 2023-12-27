using Application.UseCases.Companies.Commands.CreateCompany;

namespace Api.Tests.BuilderCommands.CompanyCommands
{
    public class CreateIdentityCommandBuilder
    {
        private string _documentType = "CC";
        private string _legalIdentifier = "123456789";

        public CreateIdentityCommandBuilder WithDocumentType(string documentType)
        {
            _documentType = documentType;
            return this;
        }

        public CreateIdentityCommandBuilder WithLegalIdentifier(string legalIdentifier)
        {
            _legalIdentifier = legalIdentifier;
            return this;
        }

        public CreateIdentityCommand Build()
        {
            return new CreateIdentityCommand(_documentType, _legalIdentifier);
        }
    }
}
