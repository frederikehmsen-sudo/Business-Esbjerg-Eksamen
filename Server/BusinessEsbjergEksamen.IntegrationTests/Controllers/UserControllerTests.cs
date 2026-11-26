namespace BusinessEsbjergEksamen.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BusinessEsbjergEksamen.IntegrationTests.Fixtures;
using Infrastructure.Entities;
using Service.DTOs;

public class UserControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    
    private const string AdminUsername = "Admin";
    private const string AdminPassword = "123";

    private async Task<string> LoginAsAdminAsync()
    {
        var response = await _client.PostAsJsonAsync("/login",
            new LoginRequest { Username = AdminUsername, Password = AdminPassword });
        response.EnsureSuccessStatusCode();

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        return login!.token;
    }

    [Fact]
    public async Task GetUsers_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/user");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Post_And_Get_Users_AsAdmin_ReturnsCreatedAndUserInList()
    {
        var token = await LoginAsAdminAsync();
        var username = $"test.user-{Guid.NewGuid():N}"[..20];
        var request = new CreateUserRequest
        {
            Username = username,
            Password = "password123",
            Role = Role.Employee
        };

        var postMessage = new HttpRequestMessage(HttpMethod.Post, "/api/user")
        {
            Content = JsonContent.Create(request)
        };
        postMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var postResponse = await _client.SendAsync(postMessage);

        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var getMessage = new HttpRequestMessage(HttpMethod.Get, "/api/user");
        getMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var getResponse = await _client.SendAsync(getMessage);
        var users = await getResponse.Content.ReadFromJsonAsync<List<UserResponse>>();

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(users);
        Assert.Contains(users, u => u.Username == username && u.Role == "Employee");
    }

    [Fact]
    public async Task SwaggerDocument_ReturnsSuccessAndValidJson()
    {
        var response = await _client.GetAsync("/swagger/v1/swagger.json");

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }
}