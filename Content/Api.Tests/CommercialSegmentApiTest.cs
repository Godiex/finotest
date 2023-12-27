using System.Net;
using Api.Tests.BuilderCommands.CommercialSegmentCommands;
using Application.UseCases.ComercialSegments.Commands.CreateCommercialSegmentId;
using Application.UseCases.ComercialSegments.Queries.GetAllCommercialSegment;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Api.Controllers;
using Domain.Entities;
using Domain.Ports;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

public class CommercialSegmentApiTest : IClassFixture<ApiApp>
{
    private readonly JsonSerializerOptions _deserializeOptions;
    private readonly HttpClient _client;
    private readonly ApiApp _apiApp;

    public CommercialSegmentApiTest(ApiApp apiApp)
    {
        _client = apiApp.CreateClient();
        _apiApp = apiApp;
        _deserializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
      
    }
    
    [Fact]
    public async Task PostNewCommercialSegmentId_ShouldBeOk()
    {
        //Arrange - Act
        var response = await _client.PostAsJsonAsync(ApiRoutes.CommercialSegments, new object());
        var responseData = JsonSerializer.Deserialize<CreateCommercialSegmentIdDto>(await response.Content.ReadAsStringAsync(), _deserializeOptions);
        
        //Assert
        response.EnsureSuccessStatusCode();
        Assert.True(responseData is not null);
        Assert.IsType<CreateCommercialSegmentIdDto>(responseData);
       
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutNewCommercialSegment_ShouldBeAccepted()
    {
        //Arrange
        var response = await _client.PostAsJsonAsync(ApiRoutes.CommercialSegments, new object());      
        var responseId = JsonSerializer.Deserialize<CreateCommercialSegmentIdDto>(await response.Content.ReadAsStringAsync(), _deserializeOptions);
        var commercialSegment = new CreateCommercialSegmentCommandBuilder().WithId(responseId!.CommercialSegmentId).Build();

        //Act 
        var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.CommercialSegments}/{responseId.CommercialSegmentId}", commercialSegment);

        //Assert
        
        Assert.True(responsePut is not null);
        Assert.Equal(HttpStatusCode.Accepted, responsePut.StatusCode);

    }
    [Fact]
    public async Task PutNewCommercialSegmentWhenNotExist_ShouldBeNotFound()
    {
        //Arrange
        var commercialSegment = new CreateCommercialSegmentCommandBuilder().Build();

        //Act 
        var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.CommercialSegments}/{Guid.NewGuid()}", commercialSegment);

        //Assert
        Assert.True(responsePut is not null);
        Assert.Equal(HttpStatusCode.NotFound, responsePut.StatusCode);

    }


    [Fact]
    public async Task PutNewCommercialSegmentWithNameAlreadyExit_ShouldBeConflict()
    {
        //Arrange - Act
        var responsePost = await _client.PostAsJsonAsync(ApiRoutes.CommercialSegments, new object());
        var serviceCollection = _apiApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
        var commercialSegmentExist = (await repository.GetAsync()).FirstOrDefault();
        var responseId = JsonSerializer.Deserialize<CreateCommercialSegmentIdDto>(await responsePost.Content.ReadAsStringAsync(), _deserializeOptions);
        var commercialSegment = new CreateCommercialSegmentCommandBuilder().WithId(responseId!.CommercialSegmentId).WithName(commercialSegmentExist!.Name).Build();

        //Assert
        var responsePut = await _client.PutAsJsonAsync($"{ApiRoutes.CommercialSegments}/{commercialSegment.Id}", commercialSegment);
        Assert.Equal(HttpStatusCode.Conflict, responsePut.StatusCode);

    }

    [Fact]
    public async Task PathcNewCommercialSegmentWithNameAlreadyExit_ShouldBeConflict()
    {
        //Arrange - Act
        var serviceCollection = _apiApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
        var commercialSegment = (await repository.GetAsync()).FirstOrDefault();
        var updatecommercialSegmentCommand = new UpdateCommercialSegmentCommandBuilder().WithName(commercialSegment!.Name).Build();
        using StringContent jsonContent = new(JsonSerializer.Serialize(updatecommercialSegmentCommand), Encoding.UTF8, "application/json");
        var responsePatch = await _client.PatchAsync($"{ApiRoutes.CommercialSegments}/{commercialSegment.Id}", jsonContent);

        //Assert
        Assert.True(responsePatch is not null);
        Assert.Equal(HttpStatusCode.Conflict, responsePatch.StatusCode);

    }

    [Fact]
    public async Task PathcNewCommercialSegment_ShouldBeOk()
    {
        //Arrange - Act
        var serviceCollection = _apiApp.GetServiceCollection();
        using var scope = serviceCollection.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<CommercialSegment>>();
        var commercialSegment = (await repository.GetAsync()).FirstOrDefault();

        var updateCommercialSegmentCommand = new UpdateCommercialSegmentCommandBuilder().WithName("ropa").Build();
        using StringContent jsonContent = new(JsonSerializer.Serialize(updateCommercialSegmentCommand), Encoding.UTF8, "application/json");
        var responsePatch = await _client.PatchAsync($"{ApiRoutes.CommercialSegments}/{commercialSegment!.Id}", jsonContent);


        //Assert
        Assert.True(responsePatch is not null);
        Assert.Equal(HttpStatusCode.OK, responsePatch.StatusCode);

    }

    [Fact]
    public async Task PathcNewCommercialSegmentWhenNotExist_ShouldBeNotFound()
    {
        //Arrange - Act
        var commercialSegment = new UpdateCommercialSegmentCommandBuilder().Build();
        using StringContent jsonContent = new(JsonSerializer.Serialize(commercialSegment), Encoding.UTF8, "application/json");
        var responsePatch = await _client.PatchAsync($"{ApiRoutes.CommercialSegments}/{Guid.NewGuid()}", jsonContent);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, responsePatch.StatusCode);
    }

    [Fact]
    public async Task DeleteCommercialSegment_ShouldBeOk()
    {
        //Arrange
        var comercialSegmentList = await _client.GetFromJsonAsync<List<CommercialSegment>>(ApiRoutes.CommercialSegments);
        //Act
        var responseDelete = await _client.DeleteAsync($"{ApiRoutes.CommercialSegments}/{comercialSegmentList!.First().Id}");

        //Assert
        responseDelete.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, responseDelete.StatusCode);
    }
        
    [Fact]
    public async Task DeleteCommercialSegmentWhenNotExist_ShouldBeNotFound()
    {
        //Arrange - Act
        var response = await _client.DeleteAsync($"{ApiRoutes.CommercialSegments}/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAllCommercialSegment_ShouldBeOk()
    {
        //Arrange
        await using var webApp = new ApiApp();
        var client = webApp.CreateClient();

        //Act
        var comercialSegmentList = await client.GetFromJsonAsync<List<GetAllCommercialSegmentDto>>(ApiRoutes.CommercialSegments);

        //assert
        Assert.True(comercialSegmentList != null);
        Assert.IsType<List<GetAllCommercialSegmentDto>>(comercialSegmentList);

    }
}