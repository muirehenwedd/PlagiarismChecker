using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;

namespace Tests.Integration.EndpointTests;

public class AdminEndpointsTest : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public AdminEndpointsTest(ApiFactory apiFactory)
    {
        _client = apiFactory.CreateClient();
        _client.DefaultRequestHeaders.Add("x-api-key", "ApiKey");
    }

    [Fact]
    public async Task UploadBaseFile_ReturnsOk_IfFileUploaded()
    {
        // Arrange
        var memoryStream = new MemoryStream("some content"u8.ToArray());
        var streamContent = new StreamContent(memoryStream)
        {
            Headers = {ContentType = new MediaTypeHeaderValue("text/plain")}
        };
        var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(streamContent, "file", "file.txt");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/admin/files")
        {
            Content = multipartFormDataContent
        };

        // Act
        var response = await _client.SendAsync(requestMessage);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteBaseFile_DeletesFileById()
    {
        // Arrange
        var memoryStream = new MemoryStream("some content"u8.ToArray());
        var streamContent = new StreamContent(memoryStream)
        {
            Headers = {ContentType = new MediaTypeHeaderValue("text/plain")}
        };
        var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(streamContent, "file", "file1.txt");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/admin/files")
        {
            Content = multipartFormDataContent
        };

        var uploadedResponse = await _client.SendAsync(requestMessage);
        var baseFile = await uploadedResponse.Content.ReadFromJsonAsync<UploadBaseFileResponse>();

        // Act
        var response = await _client.DeleteAsync($"/admin/files/{baseFile!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task DeleteBaseFile_DeletesFileByName()
    {
        // Arrange
        var memoryStream = new MemoryStream("some content"u8.ToArray());
        var streamContent = new StreamContent(memoryStream)
        {
            Headers = {ContentType = new MediaTypeHeaderValue("text/plain")}
        };
        var multipartFormDataContent = new MultipartFormDataContent();
        multipartFormDataContent.Add(streamContent, "file", "file1.txt");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/admin/files")
        {
            Content = multipartFormDataContent
        };

        var uploadedResponse = await _client.SendAsync(requestMessage);
        var baseFile = await uploadedResponse.Content.ReadFromJsonAsync<UploadBaseFileResponse>();

        // Act
        var response = await _client.DeleteAsync($"/admin/files/{baseFile!.Name}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private record UploadBaseFileResponse(Guid Id, string Name);

    [Fact]
    public async Task GetAllBaseFiles_ReturnsOk()
    {
        // Arrange

        // Act
        var getResponse = await _client.GetAsync("/admin/files");

        // Assert
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}