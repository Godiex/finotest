using Domain.Tests.BuilderEntities;
using Domain.Services.Companies.Dto;
using Domain.Enums;

namespace Domain.Tests.CompanyTest;

public class CompanyTest
{

    [Fact]
    public Task UpdateCompany_ShouldUpdateAll() 
    {
        //Arrange
        var company = new CompanyBuilder().Build();

        var authorizedAgent = new AuthorizedAgentBuilder().Build();
        const string newHostName = "colo.com";
        const CompanyState newState = CompanyState.ENABLE;
        var newCommercialSegmentId = Guid.NewGuid();
        var identity = new IdentityBuilder().Build();
        var identityToUpdate = new IdentityToUpdateDto(identity.DocumentType, identity.LegalIdentifier);
        var authorizeAgentToUpdate = new AuthorizeAgentToUpdateDto(authorizedAgent.Name, authorizedAgent.Surname, authorizedAgent.Email, identityToUpdate);

        //Act
        company.Update(newHostName, newState, newCommercialSegmentId, authorizeAgentToUpdate);

        //Assert
        Assert.Equal(newHostName, company.Hostname);
        Assert.Equal(newState, company.State);
        Assert.Equal(newCommercialSegmentId, company.CommercialSegmentId);
        Assert.Equal(authorizedAgent.Name, company.AuthorizedAgent.Name);
        Assert.Equal(authorizedAgent.Surname, company.AuthorizedAgent.Surname);
        Assert.Equal(authorizedAgent.Email, company.AuthorizedAgent.Email);
        Assert.Equal(identity.DocumentType, company.AuthorizedAgent.Identity.DocumentType);
        Assert.Equal(identity.LegalIdentifier, company.AuthorizedAgent.Identity.LegalIdentifier);
        return Task.CompletedTask;
    }
}