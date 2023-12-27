using Domain.Enums;

namespace Domain.Services.Companies.Dto;

public record CompanyToUpdateDto(
    Guid Id,
    string Hostname,
    CompanyState State,
    Guid CommercialSegmentId,
    AuthorizeAgentToUpdateDto AuthorizedAgent
);

public record AuthorizeAgentToUpdateDto(
    string Name,
    string Surname,
    string Email,
    IdentityToUpdateDto Identity
);

public record IdentityToUpdateDto(
    string DocumentType,
    string LegalIdentifier
);