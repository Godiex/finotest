using System.Linq.Expressions;
using Domain.Entities;
using Domain.Exceptions.Common;
using Domain.Ports;
using Domain.Services;
using Domain.Tests.BuilderEntities;

namespace Domain.Tests.CommercialSegmentTest;

public class CommercialSegmentServiceTest
{
    private readonly IGenericRepository<CommercialSegment> _commercialSegmentRepository;
    private readonly CommercialSegmentService _service;

    public CommercialSegmentServiceTest() { 
        _commercialSegmentRepository = Substitute.For<IGenericRepository<CommercialSegment>>();
        _service = new CommercialSegmentService(_commercialSegmentRepository);
    }

    [Fact]
    public async Task CreateNewCommercialSegmentWithValidData_ShouldAddToRepository()
    {
        // Arrange
        var commercialSegment = new CommercialSegmentBuilder().Build();
        _commercialSegmentRepository.AddAsync(Arg.Any<CommercialSegment>()).Returns(commercialSegment);
        _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(Task.FromResult(false));

        // Act
        await _service.CreateAsync(commercialSegment);

        // Assert
        await _commercialSegmentRepository.Received().Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
        await _commercialSegmentRepository.Received().AddAsync(Arg.Is<CommercialSegment>(c =>
            c == commercialSegment));
    }
 
    [Fact]
    public async Task CreateNewCommercialSegmentWithNameAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string name = "togo";
        var commercialSegment = new CommercialSegmentBuilder().WithName(name).Build();
        _commercialSegmentRepository.AddAsync(Arg.Any<CommercialSegment>()).Returns(commercialSegment);
        _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(Task.FromResult(true));
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(name), name);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(commercialSegment));

        // Assert
        await _commercialSegmentRepository.Received().Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public async Task UpdateCommercialSegmentWithValidData_ShouldUpdateToRepository()
    {
        // Arrange
        var commercialSegment = new CommercialSegmentBuilder().Build();
        _commercialSegmentRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(commercialSegment));
        _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(Task.FromResult(false));
        _commercialSegmentRepository.UpdateAsync(Arg.Any<CommercialSegment>()).Returns(Task.FromResult(commercialSegment));

        // Act
        await _service.UpdateAsync(commercialSegment.Id, commercialSegment.Name, commercialSegment.Description);

        // Assert
        await _commercialSegmentRepository.Received().Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
        await _commercialSegmentRepository.Received().GetByIdAsync(Arg.Any<Guid>());
        await _commercialSegmentRepository.Received().UpdateAsync(Arg.Any<CommercialSegment>());
    }

    [Fact]
    public async Task UpdateNewCommercialSegmentWithNameAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string name = "togo";
        var commercialSegment = new CommercialSegmentBuilder().WithName(name).Build();
        _commercialSegmentRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(commercialSegment));
        _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(true);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.UpdateAsync(commercialSegment.Id, commercialSegment.Name, commercialSegment.Description));
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(name), name);

        // Assert
        await _commercialSegmentRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
        await _commercialSegmentRepository.Received(1).Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public async Task DeleteCommercialSegmentWithValidData_ShouldDeleteToRepository()
    {
        // Arrange
        var commercialSegment = new CommercialSegmentBuilder().Build();
        _commercialSegmentRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(commercialSegment));
        _commercialSegmentRepository.DeleteAsync(Arg.Any<CommercialSegment>()).Returns(Task.FromResult(commercialSegment));

        // Act
        await _service.DeleteAsync(commercialSegment.Id);

        // Assert
        await _commercialSegmentRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
        await _commercialSegmentRepository.Received().DeleteAsync(Arg.Any<CommercialSegment>());
    }
}