using Domain.Entities;
using Domain.Tests.BuilderEntities;

namespace Domain.Tests.ContentTest;

public class ContentTest
{
    [Fact]
    public Task UpdateContent_ShouldUpdateAll()
    {
        //Arrange
        var content = new ContentBuilder().Build();
        const string newTag = "new Tag";
        const string newLogoUrl = "ww.logo.co/logo.jpg";
        List<string> newMultimedia = new List<string>
        {
            "ww.logo.co/logo.jpg",
            "ww.logo.co/logo.jpg"
        };
        List<string> newLanguages = new List<string>
        {
            "spanish",
            "english"
        };
        List<Item> newItems = new List<Item>
        {
            new Item(1,"title",new List<DynamicContent>(){new DynamicContentBuilder().Build() }),
            new Item(2,"title2",new List<DynamicContent>(){new DynamicContentBuilder().Build() })
        };
        
        //Act
        content.Update(newTag,newLogoUrl,newMultimedia,newLanguages,newItems);
        
        //Assert
        Assert.Equal(newTag, content.Tag);
        Assert.Equal(newLogoUrl, content.LogoUrl);
        Assert.Equal(newMultimedia, content.Multimedia);
        Assert.Equal(newLanguages, content.Languages);
        Assert.Equal(newItems, content.Items);

        return Task.CompletedTask;
    }
    [Fact]
    public Task UpdateContentWithNewTag_ShouldUpdateOnlyTag()
    {
        //Arrange
        var content = new ContentBuilder().Build();
        const string newTag = "new Tag";
        
        //Act
        content.Update(newTag);
        
        //Assert
        Assert.Equal(newTag, content.Tag);

        return Task.CompletedTask;
    }
    [Fact]
    public Task UpdateContentWithNewLogoUrl_ShouldUpdateOnlyLogoUrl()
    {
        //Arrange
        var content = new ContentBuilder().Build();
        const string newLogoUrl = "ww.logo.co/logo.jpg";
        
        //Act
        content.Update(content.Tag,newLogoUrl);
        
        //Assert
        Assert.Equal(newLogoUrl, content.LogoUrl);

        return Task.CompletedTask;
    }
    [Fact]
    public Task UpdateContentWithNewMultimedia_ShouldUpdateOnlyMultimedia()
    {
        //Arrange
        var content = new ContentBuilder().Build();
       
        List<string> newMultimedia = new List<string>
        {
            "ww.logo.co/logo.jpg",
            "ww.logo.co/logo.jpg"
        };
        //Act
        content.Update(content.Tag,content.LogoUrl,newMultimedia);
        
        //Assert
        Assert.Equal(newMultimedia, content.Multimedia);

        return Task.CompletedTask;
    }
    [Fact]
    public Task UpdateContentWithNewLanguages_ShouldUpdateOnlyLanguages()
    {
        //Arrange
        var content = new ContentBuilder().Build();
        List<string> newLanguages = new List<string>
        {
            "spanish",
            "english"
        };
        
        //Act
        content.Update(content.Tag,content.LogoUrl,content.Multimedia,newLanguages);
        
        //Assert
        Assert.Equal(newLanguages, content.Languages);

        return Task.CompletedTask;
    }
    [Fact]
    public Task UpdateContentWithNewItems_ShouldUpdateOnlyItems()
    {
        //Arrange
        var content = new ContentBuilder().Build();
        List<Item> newItems = new List<Item>
        {
            new Item(1,"title",new List<DynamicContent>(){new DynamicContentBuilder().Build() }),
            new Item(2,"title2",new List<DynamicContent>(){new DynamicContentBuilder().Build() })
        };
        
        //Act
        content.Update(content.Tag,content.LogoUrl,content.Multimedia,content.Languages,newItems);
        
        //Assert
        Assert.Equal(newItems, content.Items);

        return Task.CompletedTask;
    }
}