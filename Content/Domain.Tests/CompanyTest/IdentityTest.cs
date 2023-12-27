using Domain.Services.Companies.Dto;
using Domain.Tests.BuilderEntities;


namespace Domain.Tests.CompanyTest
{
    public class IdentityTest
    {
        [Fact]
        public Task UpdateIdentity_ShouldUpdateAll() 
        {
            //Arrange
            var identity = new IdentityBuilder().Build();
            const string newDocumentType = "PP";
            const string newLegalIdentifier = "32841278";
            var identityToUpdate = new IdentityToUpdateDto(newDocumentType, newLegalIdentifier);

            //Act
            identity.Update(identityToUpdate);

            //Assert
            Assert.Equal(newDocumentType, identity.DocumentType);
            Assert.Equal(newLegalIdentifier, identity.LegalIdentifier);
            return Task.CompletedTask;
        }
    }
}
