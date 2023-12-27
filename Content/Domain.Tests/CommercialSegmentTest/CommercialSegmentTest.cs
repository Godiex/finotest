using Domain.Tests.BuilderEntities;

namespace Domain.Tests.CommercialSegmentTest
{
    public class CommercialSegmentTest
    {

        [Fact]
        public Task UpdateCommercialSegment_ShouldUpdateAll()
        {
            //Arrange
            var commercialSegment = new CommercialSegmentBuilder().Build();
            const string newName = "Ropa";
            const string newDescription = "Description del segmento";

            //Act
            commercialSegment.Update(newName,newDescription);

            //Assert
            Assert.Equal(newName, commercialSegment.Name);
            Assert.Equal(newDescription, commercialSegment.Description);
            return Task.CompletedTask;
        }
        
        [Fact]
        public Task UpdateCommercialSegmentWithNewName_ShouldUpdateOnlyName()
        {
            //Arrange
            var commercialSegment = new CommercialSegmentBuilder().Build();
            const string newName = "Ropa";
            const string newDescription = "Description del segmento";


            //Act
            commercialSegment.Update(newName,commercialSegment.Description);

            //Assert
            Assert.Equal(newName, commercialSegment.Name);
            Assert.True(commercialSegment.Description != newDescription);
            return Task.CompletedTask;
        }
        
        [Fact]
        public Task UpdateCommercialSegmentWithNewDescription_ShouldUpdateOnlyDescription()
        {
            //Arrange
            var commercialSegment = new CommercialSegmentBuilder().Build();
            const string newName = "Ropa";
            const string newDescription = "Description del segmento";
            
            //Act
            commercialSegment.Update(commercialSegment.Name, newDescription);

            //Assert
            Assert.Equal(newDescription, commercialSegment.Description);
            Assert.True(commercialSegment.Name != newName);
            return Task.CompletedTask;
        }
    }
}
