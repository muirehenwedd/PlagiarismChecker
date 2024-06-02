using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using FluentAssertions;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using PlagiarismChecker.Core.Admin.Commands.UploadBaseFile;

namespace Tests.Integration.EndpointTests;

public class StudentEndpointsTest : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    private record LoginResult(string AccessToken);

    public StudentEndpointsTest(ApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();

        _client.PostAsJsonAsync("register", new
        {
            email = "testuser@email.com",
            password = "Stalker22!"
        }).Wait();

        var loginResponse = _client.PostAsJsonAsync("login", new
        {
            email = "testuser@email.com",
            password = "Stalker22!"
        }).Result;

        var testUserJwt = loginResponse.Content.ReadFromJsonAsync<LoginResult>().Result!.AccessToken;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", testUserJwt);

        // add some base files.

        var scope = apiFactory.Services.CreateScope();

        var sender = scope.ServiceProvider.GetRequiredService<ISender>();

        var memoryStream = new MemoryStream("some content"u8.ToArray());
        sender.Send(new UploadBaseFileCommand(memoryStream, MediaTypeNames.Text.Plain, "BaseFile1.txt")).AsTask()
            .Wait();
    }

    [Fact]
    public async Task CreateAssignment_ReturnsOk()
    {
        // Arrange

        // Act
        var response = await _client.PostAsJsonAsync("assignments", new
        {
            assignmentName = "Test Name"
        });

        // Assert

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateAssignment_ReturnsConflict_IfAttemptingToCreateAssignmentWithTheSameName()
    {
        // Arrange
        await _client.PostAsJsonAsync("assignments", new
        {
            assignmentName = "Test Name1"
        });

        // Act
        var response = await _client.PostAsJsonAsync("assignments", new
        {
            assignmentName = "Test Name1"
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    private record CreateAssignmentResult(Guid Id, string AssignmentName);

    [Fact]
    public async Task DeleteAssignment_DeletesAssignment()
    {
        // Arrange
        var createAssignmentResponse = await _client.PostAsJsonAsync("assignments", new
        {
            assignmentName = "Test Name4"
        });

        var createAssignmentResult = await createAssignmentResponse.Content
            .ReadFromJsonAsync<CreateAssignmentResult>();

        var memoryStream = new MemoryStream("some content"u8.ToArray());
        var streamContent = new StreamContent(memoryStream)
        {
            Headers = {ContentType = new MediaTypeHeaderValue("text/plain")}
        };
        var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(streamContent, "file", "file.txt");
        var requestMessage =
            new HttpRequestMessage(HttpMethod.Post, $"assignments/{createAssignmentResult!.Id}/files")
            {
                Content = multipartFormDataContent
            };

        await _client.SendAsync(requestMessage);

        // Act
        var response = await _client.DeleteAsync($"assignments/{createAssignmentResult!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAssignment_ReturnsNotFound_IfAssignmentWithThisIdWasNotFound()
    {
        // Arrange

        // Act
        var response = await _client.DeleteAsync($"assignments/{Guid.Empty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAssignment_ReturnsOk()
    {
        // Arrange
        var createAssignmentResponse = await _client.PostAsJsonAsync("assignments", new
        {
            assignmentName = "Test Name3"
        });

        var createAssignmentResult = await createAssignmentResponse.Content
            .ReadFromJsonAsync<CreateAssignmentResult>();

        // Act
        var response = await _client.GetAsync($"assignments/{createAssignmentResult!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAssignment_ReturnsNotFound_IfAssignmentWithThisIdWasNotFound()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync($"assignments/{Guid.Empty}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UploadAssignmentFile_ReturnsOk()
    {
        // Arrange
        var createAssignmentResponse = await _client.PostAsJsonAsync("assignments", new
        {
            assignmentName = "Test Name2"
        });

        var createAssignmentResult = await createAssignmentResponse.Content
            .ReadFromJsonAsync<CreateAssignmentResult>();

        var memoryStream = new MemoryStream("some content"u8.ToArray());
        var streamContent = new StreamContent(memoryStream)
        {
            Headers = {ContentType = new MediaTypeHeaderValue("text/plain")}
        };
        var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(streamContent, "file", "file.txt");
        var requestMessage =
            new HttpRequestMessage(HttpMethod.Post, $"assignments/{createAssignmentResult!.Id}/files")
            {
                Content = multipartFormDataContent
            };

        // Act
        var response = await _client.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CheckForPlagiarism_ReturnsOk()
    {
        // Arrange
        var createAssignmentResponse = await _client.PostAsJsonAsync("assignments", new
        {
            assignmentName = "Test Name8"
        });

        var createAssignmentResult = await createAssignmentResponse.Content
            .ReadFromJsonAsync<CreateAssignmentResult>();

        var memoryStream = new MemoryStream("some content"u8.ToArray());
        var streamContent = new StreamContent(memoryStream)
        {
            Headers = {ContentType = new MediaTypeHeaderValue("text/plain")}
        };
        var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(streamContent, "file", "file.txt");

        var uploadFileMessage =
            new HttpRequestMessage(HttpMethod.Post, $"assignments/{createAssignmentResult!.Id}/files")
            {
                Content = multipartFormDataContent
            };

        await _client.SendAsync(uploadFileMessage);

        // Act
        var response = await _client.GetAsync($"assignments/{createAssignmentResult.Id}/check");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}