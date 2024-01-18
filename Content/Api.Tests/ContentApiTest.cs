using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Controllers;
using Api.Tests.BuilderCommands;
using Application.Common.Helpers.Pagination;
using Application.UseCases.Contents.Commands.CreateContentId;
using Application.UseCases.Contents.Queries.GetAllContentsPaginated;
using Domain.Entities;
using Domain.Ports;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public class ContentApiTest:IClassFixture<ApiApp>
{
    private readonly JsonSerializerOptions _deserializeOptions;
    private readonly HttpClient _client;
    private readonly ApiApp _apiApp;

    public ContentApiTest(ApiApp apiApp)
    {
        _client = apiApp.CreateClient();
        _apiApp = apiApp;
        _deserializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
      
    }
    [Fact]
    public async Task PostNewContentId_ShouldBeOk()
    {
        //Arrange - Act
        var response = await _client.PostAsJsonAsync(ApiRoutes.Content, new object());
        var responseData = JsonSerializer.Deserialize<CreateContentIdDto>(await response.Content.ReadAsStringAsync(), _deserializeOptions);
        
        //Assert
        response.EnsureSuccessStatusCode();
        Assert.True(responseData is not null);
        Assert.IsType<CreateContentIdDto>(responseData);
       
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutNewContent_ShouldBeAccepted()
    {
        //Arrange
        var response = await _client.PostAsJsonAsync(ApiRoutes.Content, new object());      
        var responseId = JsonSerializer.Deserialize<CreateContentIdDto>(await response.Content.ReadAsStringAsync(), _deserializeOptions);
        var content = new CreateContentEntryCommandBuilder().Build();
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(content.Tag), "tag");

        // Act 
        var responsePut = await _client.PutAsync($"{ApiRoutes.Content}/{responseId?.ContentId}", formData);

        //Assert
        
        Assert.True(responsePut is not null);
        Assert.Equal(HttpStatusCode.Accepted, responsePut.StatusCode);
       
    }
    [Fact]
    public async Task PutNewContentWithLogo_ShouldBeAccepted()
    {
        //Arrange
        var response = await _client.PostAsJsonAsync(ApiRoutes.Content, new object());      
        var responseId = JsonSerializer.Deserialize<CreateContentIdDto>(await response.Content.ReadAsStringAsync(), _deserializeOptions);
        var content = new CreateContentEntryCommandBuilder().WithDefaultLogo().Build();
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(content.Tag), "tag");
        byte[] logoBytes;
        using (var logoMemoryStream = new MemoryStream())
        {
            content.Logo.CopyTo(logoMemoryStream);
            logoBytes = logoMemoryStream.ToArray();
        }

// Agregar el array de bytes al formulario
        formData.Add(new ByteArrayContent(logoBytes), "logo", content.Logo.FileName);


        // Act 
        var responsePut = await _client.PutAsync($"{ApiRoutes.Content}/{responseId?.ContentId}", formData);

        //Assert
        
        Assert.True(responsePut is not null);
        Assert.Equal(HttpStatusCode.Accepted, responsePut.StatusCode);
       
    }
    [Fact]
    public async Task UpdateContent_ShouldBeAccepted()
    {
        //Arrange
        var serviceCollection = _apiApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Content>>();
        var contentExist = (await repository.GetAsync()).FirstOrDefault();
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent("new tag"), "tag");


        // Act 
        var responsePut = await _client.PutAsync($"{ApiRoutes.Content}/{contentExist?.Id}", formData);

        //Assert
        var contentExistNow = (await repository.GetAsync()).FirstOrDefault();
        Assert.True(responsePut is not null);
        Assert.Equal(HttpStatusCode.Accepted, responsePut.StatusCode);
        Assert.False(contentExistNow?.Tag.Equals(contentExist?.Tag));

    }
    [Fact]
    public async Task PutNewContentWithTagAlreadyExit_ShouldBeConflict()
    {
        //Arrange
        var responsePost = await _client.PostAsJsonAsync(ApiRoutes.Content, new object());      
        var serviceCollection = _apiApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Content>>();
        var contentExist = (await repository.GetAsync()).FirstOrDefault();
        
        var responseId = JsonSerializer.Deserialize<CreateContentIdDto>(await responsePost.Content.ReadAsStringAsync(), _deserializeOptions);
        var content = new CreateContentEntryCommandBuilder().WithTag(contentExist.Tag).Build();
        var formData = new MultipartFormDataContent();
        formData.Add(new StringContent(content.Tag), "tag");


        // Act 
        var responsePut = await _client.PutAsync($"{ApiRoutes.Content}/{responseId?.ContentId}", formData);

        //Assert
        
        Assert.Equal(HttpStatusCode.Conflict, responsePut.StatusCode);

    }
    
    [Fact]
    public async Task GetAllPaginatedContent_ShouldBeOk()
    {
        //Arrange
        await using var webApp = new ApiApp();
        var client = webApp.CreateClient();

        //Act
        var contentListPaginated = await client.GetFromJsonAsync<PaginationResponse<GetAllContentsPaginatedDto>>(ApiRoutes.Content);

        //assert
        Assert.True(contentListPaginated != null);
        Assert.IsType<PaginationResponse<GetAllContentsPaginatedDto>>(contentListPaginated);

    }
    
    [Fact]
    public async Task DeleteCommercialSegment_ShouldBeOk()
    {
        //Arrange
        var serviceCollection = _apiApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Content>>();
        var contentExist = (await repository.GetAsync()).FirstOrDefault();
        //Act
        var responseDelete = await _client.DeleteAsync($"{ApiRoutes.Content}/{contentExist?.Id}");

        //Assert
        responseDelete.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, responseDelete.StatusCode);
        

    }
}