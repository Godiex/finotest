using Application.UseCases.Companies.Commands.UpdateCompany;

namespace Api.Tests.BuilderCommands.CompanyCommands
{
    public class UpdateIdentityCommandBuilder
    {
        private string _documentType = "CC";
        private string _legalIdentifier = "123456789";

        public UpdateIdentityCommandBuilder WithDocumentType(string documentType)
        {
            _documentType = documentType;
            return this;
        }

        public UpdateIdentityCommandBuilder WithLegalIdentifier(string legalIdentifier)
        {
            _legalIdentifier = legalIdentifier;
            return this;
        }

        public UpdateIdentityCommand Build()
        {
            return new UpdateIdentityCommand(_documentType, _legalIdentifier);
        }
    }
}
