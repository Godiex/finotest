using Domain.Tests.BuilderEntities;
using Domain.Services.Companies.Dto;

namespace Domain.Tests.CompanyTest
{
    public class AuthorizedAgentTest
    {
        [Fact]
        public Task UpdateAuthorizedAgent_ShouldUpdateAll() 
        {
            //Arrange
            var authorizedAgent = new AuthorizedAgentBuilder().Build();
            const string newName = "colo";
            const string newSurname = "colo";
            const string newEmail = "colo@gmail.com";
            var identity = new IdentityBuilder().Build();
            var identityToUpdate = new IdentityToUpdateDto(identity.DocumentType, identity.LegalIdentifier);
            var authorizeAgentToUpdate = new AuthorizeAgentToUpdateDto(newName, newSurname, newEmail, identityToUpdate); 
            
            //Act
            authorizedAgent.Update(authorizeAgentToUpdate);

            //Assert
            Assert.Equal(newName, authorizedAgent.Name);
            Assert.Equal(newSurname, authorizedAgent.Surname);
            Assert.Equal(newEmail, authorizedAgent.Email);
            Assert.Equal(identity.DocumentType, authorizedAgent.Identity.DocumentType);
            Assert.Equal(identity.LegalIdentifier, authorizedAgent.Identity.LegalIdentifier);
            return Task.CompletedTask;
        }
    }
}
