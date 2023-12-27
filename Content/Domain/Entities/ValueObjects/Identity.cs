using Domain.Services.Companies.Dto;

namespace Domain.Entities.ValueObjects;

public class Identity : ValueObject
{
    public string DocumentType { get; set; }
    public string LegalIdentifier { get; set; }

    public Identity()
    {

    }

    public Identity(string documentType, string legalIdentifier)
    {
        DocumentType = documentType;
        LegalIdentifier = legalIdentifier;
    }

    public void Update(IdentityToUpdateDto identityToUpdate)
    {
        DocumentType = identityToUpdate.DocumentType;
        LegalIdentifier = identityToUpdate.LegalIdentifier;
    }
}