using Domain.Entities.ValueObjects;

namespace Domain.Tests.BuilderEntities;

public class IdentityBuilder
{
    private string _documentType = "CC";
    private string _legalIdentifier = "123456789";

    public IdentityBuilder WithDocumentType(string documentType)
    {
        _documentType = documentType;
        return this;
    }

    public IdentityBuilder WithLegalIdentifier(string legalIdentifier)
    {
        _legalIdentifier = legalIdentifier;
        return this;
    }

    public Identity Build()
    {
        return new Identity(_documentType, _legalIdentifier);
    }
}