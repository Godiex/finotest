using System.Text.Json.Serialization;

namespace Application.UseCases.Companies.Commands.CreateCompany;

public record CreateCompanyCommand : IRequest<Unit>
{
    public CreateCompanyCommand(Guid id, string name, string legalIdentifier, string hostname, CreateAuthorizedAgentCommand authorizedAgent, Guid commercialSegment)
    {
        Id = id;
        Name = name;
        LegalIdentifier = legalIdentifier;
        Hostname = hostname;
        AuthorizedAgent = authorizedAgent;
        CommercialSegment = commercialSegment;
    }

    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LegalIdentifier { get; set; }
    public string Hostname { get; set; }
    public CreateAuthorizedAgentCommand AuthorizedAgent { get; set; }
    public Guid CommercialSegment { get; set; }
}


public record CreateAuthorizedAgentCommand(
    string Name,
    string Surname,
    string Email,
    CreateIdentityCommand Identity
);

public record CreateIdentityCommand(
    string DocumentType,
    string LegalIdentifier
);
