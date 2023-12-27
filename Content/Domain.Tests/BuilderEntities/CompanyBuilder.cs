using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Domain.Tests.BuilderEntities;

public class CompanyBuilder
{
    private string _name = "Togo";
    private string _legalIdentifier = "1234567889";
    private string _hostname = "togo.com";
    private AuthorizedAgent _authorizedAgent = new AuthorizedAgentBuilder().Build();
    private Guid _commercialSegmentId;
    private CommercialSegment _commercialSegment = new CommercialSegmentBuilder().Build();

    public CompanyBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CompanyBuilder WithLegalIdentifier(string legalIdentifier)
    {
        _legalIdentifier = legalIdentifier;
        return this;
    }

    public CompanyBuilder WithHostname(string hostname)
    {
        _hostname = hostname;
        return this;
    }

    public CompanyBuilder WithAuthorizedAgent(AuthorizedAgent authorizedAgent)
    {
        _authorizedAgent = authorizedAgent;
        return this;
    }

    public CompanyBuilder WithCommercialSegmentId(Guid commercialSegmentId)
    {
        _commercialSegmentId = commercialSegmentId;
        return this;
    }
    
    public CompanyBuilder WithCommercialSegment(CommercialSegment commercialSegment)
    {
        _commercialSegment = commercialSegment;
        return this;
    }

    public Company Build()
    {
        return new Company(
            Guid.NewGuid(),
            _name,
            _legalIdentifier,
            _hostname,
            _authorizedAgent,
            _commercialSegmentId
        )
        {
            CommercialSegment = _commercialSegment
        };
    }
}