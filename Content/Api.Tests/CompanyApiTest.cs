using Api.Tests.BuilderCommands.CompanyCommands;
using Application.UseCases.Companies.Commands.CreateCompanyId;
using Domain.Entities;
using Domain.Ports;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Api.Controllers;

namespace Api.Tests
{
    public class CompanyApiTest : IClassFixture<ApiApp>
    {
        private readonly JsonSerializerOptions _deserializeOptions;
        private readonly HttpClient _client;
        private readonly ApiApp _apiApp;
        public CompanyApiTest(ApiApp apiApp) 
        {
            _client = apiApp.CreateClient();
            _apiApp = apiApp;
            _deserializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        }

        [Fact]
        public async Task PostNewCompanyId_ShouldBeOk()
        {
            //Arrange - Act
            var response = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseData = JsonSerializer.Deserialize<CreateCompanyIdDto>(await response.Content.ReadAsStringAsync(), _deserializeOptions);
            
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.True(responseData is not null);
            Assert.IsType<CreateCompanyIdDto>(responseData);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact]
        public async Task PutNewCompany_ShouldBeAccepted()
        {
            //Arrange
            var responseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await responseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var commercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);
            var identity = new CreateIdentityCommandBuilder().WithLegalIdentifier("328412777").Build();
            var authorizedAgent = new CreateAuthorizedAgentCommandBuilder().WithIdentity(identity).WithEmail("jedev3@gmail.com").Build();
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(commercialSegmentList!.First().Id).WithAuthorizedAgent(authorizedAgent).WithName("uniqueName3")
                .WithLegalIdentifier("3284127777")
                .WithHostname("unique3.com").Build();
           

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert

            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.Accepted, responsePut.StatusCode);

        }
        [Fact]
        public async Task PutNewCompanyWhitNameAlreadyExist_ShouldBeConflict()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var responseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await responseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var commercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(commercialSegmentList!.First().Id).WithName(companyExist!.Name).Build();

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert
           
            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.Conflict, responsePut.StatusCode);

        }

        [Fact]
        public async Task PutNewCompanyWhitEmailAuthorizedAgentAlreadyExist_ShouldBeConflict()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var reponseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await reponseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var comercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);
            var authorizedAgent = new CreateAuthorizedAgentCommandBuilder().WithEmail(companyExist!.AuthorizedAgent.Email).Build();
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(comercialSegmentList!.First().Id).WithAuthorizedAgent(authorizedAgent).Build();
            
            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert
            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.Conflict, responsePut.StatusCode);

        }

        [Fact]
        public async Task PutNewCompanyWhitLegalIdentifierAlreadyExist_ShouldBeConflict()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var reponseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await reponseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var comercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);
           
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(comercialSegmentList!.First().Id).WithLegalIdentifier(companyExist!.LegalIdentifier).Build();

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert

            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.Conflict, responsePut.StatusCode);

        }

        [Fact]
        public async Task PutNewCompanyWhitDocumentTypeInvalid_ShouldBeNotFound()
        {
            //Arrange
            
            var reponseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await reponseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var comercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);

            var identity = new CreateIdentityCommandBuilder().WithDocumentType("TT").Build();
            var authorizedAgent = new CreateAuthorizedAgentCommandBuilder().WithIdentity(identity).Build();
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(comercialSegmentList!.First().Id).WithAuthorizedAgent(authorizedAgent).Build();

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert

            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.NotFound, responsePut.StatusCode);

        }

        [Fact]
        public async Task PutNewCompanyWhitHostNameAlreadyExist_ShouldBeConflict()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var reponseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await reponseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var comercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);

            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(comercialSegmentList!.First().Id).WithHostname(companyExist!.Hostname).Build();

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert

            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.Conflict, responsePut.StatusCode);

        }

        [Fact]
        public async Task PutNewCompanyWhitHostNameInvalid_ShouldBeBadRequest()
        {
            //Arrange
            var reponseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await reponseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var comercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);

            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(comercialSegmentList!.First().Id).WithHostname("comadotcom").Build();

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert

            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.BadRequest, responsePut.StatusCode);

        }

        [Fact]
        public async Task PutNewCompanyWhitCommercialSegmentInvalid_ShouldBeNotFound()
        {
            //Arrange
            var reponseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await reponseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(Guid.NewGuid()).Build();

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert
            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.NotFound, responsePut.StatusCode);

        }

        [Fact]
        public async Task PutNewCompanyWhitIdentityAlreadyExist_ShouldBeConflict()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var responseCreateCompanyId = await _client.PostAsJsonAsync(ApiRoutes.Companies, new object());
            var responseCompanyId = JsonSerializer.Deserialize<CreateCompanyIdDto>(await responseCreateCompanyId.Content.ReadAsStringAsync(), _deserializeOptions);
            var comercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);
            var identity = new CreateIdentityCommandBuilder().WithLegalIdentifier(companyExist!.AuthorizedAgent.Identity.LegalIdentifier).Build();
            var authorizedAgent = new CreateAuthorizedAgentCommandBuilder().WithIdentity(identity).Build();
            var company = new CreateCompanyCommandBuilder().WithCommercialSegmentId(comercialSegmentList!.First().Id).WithAuthorizedAgent(authorizedAgent).Build();

            //Act
            var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.Companies}/{responseCompanyId!.CompanyId}", company);

            //Assert
            Assert.True(responsePut is not null);
            Assert.Equal(HttpStatusCode.Conflict, responsePut.StatusCode);

        }


        [Fact]
        public async Task PatchNewCompany_ShouldBeOk()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var repositoryCommercialSegment = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var commercialSegment = (await repositoryCommercialSegment.GetAsync()).FirstOrDefault();
            
            //Act
            var companyCommand = new UpdateCompanyCommandBuilder().WithCommercialSegmentId(commercialSegment!.Id).Build();

            //Assert
            using StringContent jsonContent = new(JsonSerializer.Serialize(companyCommand), Encoding.UTF8,"application/json");
            var responsePatch = await _client.PatchAsync($"{ApiRoutes.Companies}/{companyExist!.Id}", jsonContent);
            Assert.True(responsePatch is not null);
            Assert.Equal(HttpStatusCode.OK, responsePatch.StatusCode);
        }

        [Fact]
        public async Task PatchNewCompanyInvalid_ShouldBeNotFound()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repositoryCommercialSegment = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();   
            var commercialSegment = (await repositoryCommercialSegment.GetAsync()).FirstOrDefault();

            //Act
            var companyCommand = new UpdateCompanyCommandBuilder().WithCommercialSegmentId(commercialSegment!.Id).Build();

            //Assert
            using StringContent jsonContent = new(JsonSerializer.Serialize(companyCommand), Encoding.UTF8, "application/json");
            var responsePatch = await _client.PatchAsync($"{ApiRoutes.Companies}/{Guid.NewGuid()}", jsonContent);
            Assert.True(responsePatch is not null);
            Assert.Equal(HttpStatusCode.NotFound, responsePatch.StatusCode);
        }

        [Fact]
        public async Task PatchNewCompanyWithInvalidCommercialSegment_ShouldBeNotFound()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();

            //Act
            var companyCommand = new UpdateCompanyCommandBuilder().Build();

            //Assert
            using StringContent jsonContent = new(JsonSerializer.Serialize(companyCommand), Encoding.UTF8, "application/json");
            var responsePatch = await _client.PatchAsync($"{ApiRoutes.Companies}/{companyExist!.Id}", jsonContent);
            Assert.True(responsePatch is not null);
            Assert.Equal(HttpStatusCode.NotFound, responsePatch.StatusCode);
        }

        [Fact]
        public async Task PatchNewCompanyWithAuthoeizedAgentLegalIdentifierAlreadyExist_ShouldBeConflict()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var repositoryCommercialSegment = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var commercialSegment = (await repositoryCommercialSegment.GetAsync()).FirstOrDefault();

            //Act
            var identity = new UpdateIdentityCommandBuilder().WithLegalIdentifier("1066865371").Build();
            var authorizedAgent = new UpdateAuthorizedAgentCommandBuilder().WithIdentity(identity).Build();
            var companyCommand = new UpdateCompanyCommandBuilder().WithCommercialSegmentId(commercialSegment!.Id).WithAuthorizedAgent(authorizedAgent).Build();

            //Assert
            using StringContent jsonContent = new(JsonSerializer.Serialize(companyCommand), Encoding.UTF8, "application/json");
            var responsePatch = await _client.PatchAsync($"{ApiRoutes.Companies}/{companyExist!.Id}", jsonContent);
            Assert.True(responsePatch is not null);
            Assert.Equal(HttpStatusCode.Conflict, responsePatch.StatusCode);
        }

        [Fact]
        public async Task PatchNewCompanyWithAuthoeizedAgentDocumentTypeInvalid_ShouldBeNotFound()
        {
            //Arrange
            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var repositoryCommercialSegment = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var commercialSegment = (await repositoryCommercialSegment.GetAsync()).FirstOrDefault();

            //Act
            var identity = new UpdateIdentityCommandBuilder().WithDocumentType("TA").Build();
            var authorizedAgent = new UpdateAuthorizedAgentCommandBuilder().WithIdentity(identity).Build();
            var companyCommand = new UpdateCompanyCommandBuilder().WithCommercialSegmentId(commercialSegment!.Id).WithAuthorizedAgent(authorizedAgent).Build();

            //Assert
            using StringContent jsonContent = new(JsonSerializer.Serialize(companyCommand), Encoding.UTF8, "application/json");
            var responsePatch = await _client.PatchAsync($"{ApiRoutes.Companies}/{companyExist!.Id}", jsonContent);
            Assert.True(responsePatch is not null);
            Assert.Equal(HttpStatusCode.NotFound, responsePatch.StatusCode);
        }

        [Fact]
        public async Task PatchNewCompanyWithHostNameInvalid_ShouldBeBadRequest()
        {
            //Arrange

            var serviceCollection = _apiApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
            var repositoryCommercialSegment = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
            var companyExist = (await repository.GetAsync()).FirstOrDefault();
            var commercialSegment = (await repositoryCommercialSegment.GetAsync()).FirstOrDefault();

            //Act
            var companyCommand = new UpdateCompanyCommandBuilder().WithCommercialSegmentId(commercialSegment!.Id).WithHostname("hostdotcom").Build();

            //Assert
            using StringContent jsonContent = new(JsonSerializer.Serialize(companyCommand), Encoding.UTF8, "application/json");
            var responsePatch = await _client.PatchAsync($"{ApiRoutes.Companies}/{companyExist!.Id}", jsonContent);
            Assert.True(responsePatch is not null);
            Assert.Equal(HttpStatusCode.BadRequest, responsePatch.StatusCode);
        }

    }
}
