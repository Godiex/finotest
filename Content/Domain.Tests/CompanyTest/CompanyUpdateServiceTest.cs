using Domain.Entities;
using Domain.Exceptions;
using Domain.Exceptions.Common;
using Domain.Ports;
using Domain.Services.Companies;
using Domain.Services.Companies.Dto;
using Domain.Tests.BuilderEntities;
using System.Linq.Expressions;


namespace Domain.Tests.CompanyTest
{
    public class CompanyUpdateServiceTest
    {
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly IGenericRepository<DocumentType> _documentTypeRepository;
        private readonly IGenericRepository<CommercialSegment> _commercialSegmentRepository;
        private readonly CompanyUpdateService _service;

        public CompanyUpdateServiceTest()
        {
            _companyRepository = Substitute.For<IGenericRepository<Company>>();
            _documentTypeRepository = Substitute.For<IGenericRepository<DocumentType>>();
            _commercialSegmentRepository = Substitute.For<IGenericRepository<CommercialSegment>>();
            var companyValidationService = new CompanyValidationService(_companyRepository, _commercialSegmentRepository, _documentTypeRepository);
            _service = new CompanyUpdateService(_companyRepository, companyValidationService);

        }

        [Fact]
        public async Task UpdateCompanyWithValidData_ShouldAddToRepository()
        {
            // Arrange
            var company = new CompanyBuilder().Build();
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(company));
           
            // Act
            await _service.UpdateAsync(companyToUpdate);

            // Assert
            await _companyRepository.Received().UpdateAsync(Arg.Any<Company>());
        }
        
        [Fact]
        public async Task UpdateCompanyWithValidNewHostName_ShouldAddToRepository()
        {
            // Arrange
            const string host = "lago.com";
            var company = new CompanyBuilder().WithHostname(host).Build();
            var companyOld = new CompanyBuilder().Build();
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));
            _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(false));

            // Act
            await _service.UpdateAsync(companyToUpdate);

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            await _companyRepository.Received().Exist(Arg.Any<Expression<Func<Company, bool>>>());
            await _companyRepository.Received().UpdateAsync(Arg.Any<Company>());
        }
        
        [Fact]
        public async Task UpdateCompanyWithDifferentAuthorizedAgent_ShouldAddToRepository()
        {
            // Arrange
            var idenity = new IdentityBuilder().WithDocumentType("PP").WithLegalIdentifier("1066865").Build();
            var authorizedAgent = new AuthorizedAgentBuilder().WithIdentity(idenity).Build();
            var company = new CompanyBuilder().WithAuthorizedAgent(authorizedAgent).Build();
            var companyOld = new CompanyBuilder().Build();
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));
            _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(false));
            _documentTypeRepository.Exist(Arg.Any<Expression<Func<DocumentType, bool>>>()).Returns(Task.FromResult(true));

            // Act
            await _service.UpdateAsync(companyToUpdate);

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            await _companyRepository.Received(1).Exist(Arg.Any<Expression<Func<Company, bool>>>());
            await _documentTypeRepository.Received(1).Exist(Arg.Any<Expression<Func<DocumentType, bool>>>());
            await _companyRepository.Received().UpdateAsync(Arg.Any<Company>());

        }
        
        [Fact]
        public async Task UpdateCompanyWithDifferentCommercialSegment_ShouldAddToRepository()
        {
            // Arrange
            var id = Guid.NewGuid();
            var company = new CompanyBuilder().WithCommercialSegmentId(id).Build();
            var companyOld = new CompanyBuilder().Build();
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(true);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));

            // Act
            await _service.UpdateAsync(companyToUpdate);

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            await _commercialSegmentRepository.Received(1).Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
            await _companyRepository.Received().UpdateAsync(Arg.Any<Company>());

        }

        [Fact]
        public async Task UpdateCompanyWithHostNameAlreadyExist_ShouldThrowResourceAlreadyExistException()
        {
            // Arrange
            const string host = "lago.com";
            var company = new CompanyBuilder().WithHostname(host).Build();
            var companyOld = new CompanyBuilder().Build();
            var exceptionMessage = string.Format(Messages.AlredyExistException, nameof(host), host);
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));
            _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(true));

            // Act
            var exception = await Record.ExceptionAsync(() => _service.UpdateAsync(companyToUpdate));

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            await _companyRepository.Received(1).Exist(Arg.Any<Expression<Func<Company, bool>>>());
            Assert.NotNull(exception);
            Assert.IsType<ResourceAlreadyExistException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task UpdateCompanyWithInvalidateHostName_ShouldThroInvalidHostNameException()
        {
            // Arrange
            const string host = "lagodotcom";
            var company = new CompanyBuilder().WithHostname(host).Build();
            var companyOld = new CompanyBuilder().Build();
            var exceptionMessage = Messages.InvalidHostException;
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));
           
            // Act
            var exception = await Record.ExceptionAsync(() => _service.UpdateAsync(companyToUpdate));

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            Assert.NotNull(exception);
            Assert.IsType<InvalidHostNameException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);

        }
        
        [Fact]
        public async Task UpdateCompanyWithInvalidCommercialSegment_ShouldThrowResourceNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var company = new CompanyBuilder().WithCommercialSegmentId(id).Build();   
            var companyOld = new CompanyBuilder().Build();
            var exceptionMessage = string.Format(Messages.ResourceNotFoundException, id);
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _commercialSegmentRepository.Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>()).Returns(false);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));

            // Act
            var exception = await Record.ExceptionAsync(() => _service.UpdateAsync(companyToUpdate));

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            await _commercialSegmentRepository.Received(1).Exist(Arg.Any<Expression<Func<CommercialSegment, bool>>>());
            Assert.NotNull(exception);
            Assert.IsType<ResourceNotFoundException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);

        }
        
        [Fact]
        public async Task UpdateCompanyWithAuthorizedAgentLegalIdentifierAlreadyExist_ShouldThrowResourceAlreadyExistException()
        {
            // Arrange
            const string authorizedAgentLegalIdentifier = "1066865";
            var idenity = new IdentityBuilder().WithDocumentType("PP").WithLegalIdentifier(authorizedAgentLegalIdentifier).Build();
            var authorizedAgent = new AuthorizedAgentBuilder().WithIdentity(idenity).Build();
            var company = new CompanyBuilder().WithAuthorizedAgent(authorizedAgent).Build();
            var companyOld = new CompanyBuilder().Build();
            var exceptionMessage = string.Format(Messages.AlredyExistException,nameof(authorizedAgentLegalIdentifier), authorizedAgentLegalIdentifier);
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));
            _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(true));

            // Act
            var exception = await Record.ExceptionAsync(() => _service.UpdateAsync(companyToUpdate));

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            await _companyRepository.Received(1).Exist(Arg.Any<Expression<Func<Company, bool>>>());

            Assert.NotNull(exception);
            Assert.IsType<ResourceAlreadyExistException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);

        }
        [Fact]
        public async Task UpdateCompanyWithInvalidAuthorizedAgentLegalDocumentType_ShouldThrowResourceNotFoundException()
        {
            // Arrange
            const string documentType = "LP";
            var idenity = new IdentityBuilder().WithDocumentType(documentType).WithLegalIdentifier("1066865").Build();
            var authorizedAgent = new AuthorizedAgentBuilder().WithIdentity(idenity).Build();
            var company = new CompanyBuilder().WithAuthorizedAgent(authorizedAgent).Build();
            var companyOld = new CompanyBuilder().Build();
            var exceptionMessage = string.Format(Messages.ResourceNotFoundException, documentType);
            var identityToUpdate = new IdentityToUpdateDto(company.AuthorizedAgent.Identity.DocumentType, company.AuthorizedAgent.Identity.LegalIdentifier);
            var authorizedAgentToupdate = new AuthorizeAgentToUpdateDto(company.AuthorizedAgent.Name, company.AuthorizedAgent.Surname, company.AuthorizedAgent.Email, identityToUpdate);
            var companyToUpdate = new CompanyToUpdateDto(Guid.NewGuid(), company.Hostname, company.State, company.CommercialSegmentId, authorizedAgentToupdate);
            _companyRepository.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(companyOld));
            _companyRepository.Exist(Arg.Any<Expression<Func<Company, bool>>>()).Returns(Task.FromResult(false));
            _documentTypeRepository.Exist(Arg.Any<Expression<Func<DocumentType, bool>>>()).Returns(Task.FromResult(false));

            // Act
            var exception = await Record.ExceptionAsync(() => _service.UpdateAsync(companyToUpdate));

            // Assert
            await _companyRepository.Received(1).GetByIdAsync(Arg.Any<Guid>());
            await _companyRepository.Received(1).Exist(Arg.Any<Expression<Func<Company, bool>>>());
            await _documentTypeRepository.Received(1).Exist(Arg.Any<Expression<Func<DocumentType, bool>>>());
            Assert.NotNull(exception);
            Assert.IsType<ResourceNotFoundException>(exception);
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }
}
