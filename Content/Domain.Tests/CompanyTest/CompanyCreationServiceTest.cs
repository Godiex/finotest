using System.Linq.Expressions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Common;
using Domain.Ports;
using Domain.Services.Companies;
using Domain.Tests.BuilderEntities;

namespace Domain.Tests.CompanyTest;

public class CompanyCreationServiceTest
{
    private readonly IGenericRepository<Company> _companyRepository;
    private readonly IGenericRepository<DocumentType> _documentTypeRepository;
    private readonly IGenericRepository<CommercialSegment> _commercialSegmentRepository;

    private readonly CompanyCreationService _service;

    public CompanyCreationServiceTest()
    {
        _companyRepository = Substitute.For<IGenericRepository<Company>>();
        _documentTypeRepository = Substitute.For<IGenericRepository<DocumentType>>();
        _commercialSegmentRepository = Substitute.For<IGenericRepository<CommercialSegment>>();

        var companyValidationService = new CompanyValidationService(_companyRepository,_commercialSegmentRepository, _documentTypeRepository);
        _service = new CompanyCreationService(_companyRepository, companyValidationService);
    }

    [Fact]
    public async Task CreateNewCompanyWithAuthorizedAgentEmailAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string email = "email@gmail.com";
        var authorizedAgent = new AuthorizedAgentBuilder().WithEmail(email).Build();
        var company = new CompanyBuilder().WithAuthorizedAgent(authorizedAgent).Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(true);
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(email), email);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        // Assert
        await _companyRepository.Received().Exist(Arg.Any<Expression<Func<Company, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public async Task CreateNewCompanyWithInvalidDocumentType_ShouldThrowResourceNotFoundException()
    {
        // Arrange
        const string documentType = "NN";
        var exceptionMessage = string.Format(Messages.ResourceNotFoundException, documentType);
        var identity = new IdentityBuilder().WithDocumentType(documentType).Build();
        var authorizedAgent = new AuthorizedAgentBuilder().WithIdentity(identity).Build();
        var company = new CompanyBuilder().WithAuthorizedAgent(authorizedAgent).Build();
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(false));
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _documentTypeRepository.Exist(Arg.Any<Expression<Func<DocumentType, bool>>>()).Returns(Task.FromResult(false));
        
        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        // Assert
        await _companyRepository.Received().Exist(Arg.Any<Expression<Func<Company, bool>>>());
        await _documentTypeRepository.Received().Exist(Arg.Any<Expression<Func<DocumentType, bool>>>());


        Assert.NotNull(exception);
        Assert.IsType<ResourceNotFoundException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public async Task CreateNewCompanyWithLegalIdentifierAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string companyLegalIdentifier = "32841275";
        var company = new CompanyBuilder().WithLegalIdentifier(companyLegalIdentifier).Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(false, false, true);
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(companyLegalIdentifier), companyLegalIdentifier);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        // Assert
        await _companyRepository.Received(3).Exist(Arg.Any<Expression<Func<Company, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public async Task CreateNewCompanyWithHostNameIsInvalid_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string invalidHostName = "test";
        var company = new CompanyBuilder().WithHostname(invalidHostName).Build();
        _documentTypeRepository.Exist(Arg.Any<Expression<Func<DocumentType, bool>>>()).Returns(true);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        // Assert
        await _companyRepository.Received().Exist(Arg.Any<Expression<Func<Company, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<InvalidHostNameException>(exception);
        Assert.Equal(Messages.InvalidHostException, exception.Message);
    }

    [Fact]
    public async Task CreateNewCompanyWithValidData_ShouldAddToRepository()
    {
        // Arrange
        var company = new CompanyBuilder().Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(false));
        _documentTypeRepository.Exist(Arg.Any<Expression<Func<DocumentType, bool>>>()).Returns(true);
        _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(true);

       // Act
       await _service.CreateAsync(company);
       
        // Assert
        await _documentTypeRepository.Received().Exist(Arg.Any<Expression<Func<DocumentType, bool>>>());
        await _companyRepository.Received(5).Exist(Arg.Any<Expression<Func<Company, bool>>>());
        await _commercialSegmentRepository.Received().Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
        await _companyRepository.Received().AddAsync(Arg.Is<Company>(c =>
            c.AuthorizedAgent.Email == company.AuthorizedAgent.Email &&
            c.AuthorizedAgent.Identity.LegalIdentifier == company.AuthorizedAgent.Identity.LegalIdentifier &&
            c.Hostname == company.Hostname &&
            c.Name == company.Name &&
            c.LegalIdentifier == company.LegalIdentifier &&
            c.CommercialSegmentId == company.CommercialSegmentId));
    }

    [Fact]
    public async Task CreateNewCompanyWithAuthorizedAgentHaveInvalidEmail_ShouldThrowInvalidEmailException()
    {
        // Arrange
        const string invalidEmail = "email@test";
        var authorizedAgent = new AuthorizedAgentBuilder().WithEmail(invalidEmail).Build();
        var company = new CompanyBuilder().WithAuthorizedAgent(authorizedAgent).Build();

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<InvalidEmailException>(exception);
        Assert.Equal(Messages.InvalidHostException, Messages.InvalidHostException);
    }


    [Fact]
    public async Task CreateNewCompanyWithHostNameAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        var host = "togo.com";
        var company = new CompanyBuilder().WithHostname(host).Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(false,false,false,true);
        _documentTypeRepository.Exist(Arg.Any<Expression<Func<DocumentType, bool>>>()).Returns(true);
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(host), host);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        // Assert
        await _companyRepository.Received(4).Exist(Arg.Any<Expression<Func<Company, bool>>>());
        await _documentTypeRepository.Received().Exist(Arg.Any<Expression<Func<DocumentType, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }


    [Fact]
    public async Task CreateNewCompanyWithNameAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string name = "togo";
        var company = new CompanyBuilder().WithName(name).Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(false,  true);
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(name), name);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        // Assert
        await _companyRepository.Received(2).Exist(Arg.Any<Expression<Func<Company, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }


    [Fact]
    public async Task CreateNewCompanyWithAuthorizedAgentLegalIdentifierAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        const string companyLegalIdentifier = "togo";
        var company = new CompanyBuilder().WithLegalIdentifier(companyLegalIdentifier).Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(false, false, true);
        var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(companyLegalIdentifier), companyLegalIdentifier);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));

        // Assert
        await _companyRepository.Received(3).Exist(Arg.Any<Expression<Func<Company, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceAlreadyExistException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }

    [Fact]
    public async Task CreateNewCompanyWithCommercialSegmentAlreadyExist_ShouldThrowResourceAlreadyExistException()
    {
        // Arrange
        var commercialSegment = new CommercialSegmentBuilder().Build();
        var company = new CompanyBuilder().WithCommercialSegmentId(commercialSegment.Id).Build();
        _companyRepository.AddAsync(Arg.Any<Company>()).Returns(company);
        _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(false));
        _documentTypeRepository.Exist(Arg.Any<Expression<Func<DocumentType, bool>>>()).Returns(true);
        _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(false);

        // Act
        var exception = await Record.ExceptionAsync(() => _service.CreateAsync(company));
        var exceptionMessage = string.Format(Messages.ResourceNotFoundException, company.CommercialSegmentId);

        // Assert
        await _documentTypeRepository.Received().Exist(Arg.Any<Expression<Func<DocumentType, bool>>>());
        await _companyRepository.Received().Exist(Arg.Any<Expression<Func<Company, bool>>>());
        await _commercialSegmentRepository.Received().Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
        Assert.NotNull(exception);
        Assert.IsType<ResourceNotFoundException>(exception);
        Assert.Equal(exceptionMessage, exception.Message);
    }


}