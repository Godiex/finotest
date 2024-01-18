using Domain.Entities;
using System.Linq.Expressions;
using Domain.Exceptions.Common;
using Domain.Ports;
using Domain.Services;
using Domain.Tests.BuilderEntities;


namespace Domain.Tests.ContentTest;

public class ContentServiceTest
{
    private readonly IGenericRepository<Content> _contentRepository;
    private readonly ContentService _service;

    public ContentServiceTest() { 
        _contentRepository = Substitute.For<IGenericRepository<Content>>();
        _service = new ContentService(_contentRepository);
    }
    [Fact]
    public async Task CreateNewContentWithValidData_ShouldAddToRepository()
    {
        // Arrange
        var content = new ContentBuilder().Build();
        _contentRepository.AddAsync(Arg.Any<Content>()).Returns(content);
        _contentRepository.Exist(Arg.Any<Expression<Func<Content, bool>>>()).Returns(Task.FromResult(false));

        // Act
        await _service.CreateAsync(content);

        // Assert
        await _contentRepository.Received().Exist(Arg.Any<Expression<Func<Content, bool>>>());
        await _contentRepository.Received().AddAsync(Arg.Is<Content>(c =>
            c == content));
    }
    [Fact]
    public async Task CreateNewContentWithTagAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string tag = "tag";
        var content = new ContentBuilder().WithTag(tag).Build();
        _contentRepository.AddAsync(Arg.Any<Content>()).Returns(content);
        _contentRepository.Exist(Arg.Any<Expression<Func<Content, bool>>>()).Returns(Task.FromResult(true));
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(tag), tag);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(content));

        // Assert
        await _contentRepository.Received().Exist(Arg.Any<Expression<Func<Content, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }
    [Fact]
    public async Task DeleteContentWithValidData_ShouldDeleteToRepository()
    {
        // Arrange
        var content = new ContentBuilder().Build();
        _contentRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(content));
        _contentRepository.DeleteAsync(Arg.Any<Content>()).Returns(Task.FromResult(content));

        // Act
        await _service.DeleteAsync(content.Id);

        // Assert
        await _contentRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
        await _contentRepository.Received().DeleteAsync(Arg.Any<Content>());
    }
}