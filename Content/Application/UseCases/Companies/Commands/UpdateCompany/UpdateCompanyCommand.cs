using System.Text.Json.Serialization;
using Domain.Enums;

namespace Application.UseCases.Companies.Commands.UpdateCompany;

public record UpdateCompanyCommand: IRequest<Unit>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Hostname { get; set; }
    public Guid CommercialSegment { get; set; }
    
    public CompanyState State { get; set; }
    
    public UpdateAuthorizeAgentCommand AuthorizedAgent { get; set; }

    public UpdateCompanyCommand(Guid id, string hostname, Guid commercialSegment, CompanyState state, UpdateAuthorizeAgentCommand authorizedAgent)
    {
        Id = id;
        Hostname = hostname;
        CommercialSegment = commercialSegment;
        State = state;
        AuthorizedAgent = authorizedAgent;
    }
}

public record UpdateAuthorizeAgentCommand(
    string Name,
    string Surname,
    string Email,
    UpdateIdentityCommand Identity
);

public record UpdateIdentityCommand(
    string DocumentType,
    string LegalIdentifier
);