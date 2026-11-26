namespace BusinessEsbjergEksamen.IntegrationTests.Fixtures;

using System.Net.Http.Headers;
using System.Net.Http.Json;
using Service.DTOs;


public static class AuthHelper
{
    public static Task<HttpClient> CreateAdminClientAsync(this CustomWebApplicationFactory factory) =>
        CreateLoggedInClientAsync(factory, "Admin", "123");

    public static Task<HttpClient> CreateAccountantClientAsync(this CustomWebApplicationFactory factory) =>
        CreateLoggedInClientAsync(factory, "Accountant", "123");

    private static async Task<HttpClient> CreateLoggedInClientAsync(
        CustomWebApplicationFactory factory, string username, string password)
    {
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/login",
            new LoginRequest { Username = username, Password = password });
        response.EnsureSuccessStatusCode();

        var login = await response.Content.ReadFromJsonAsync<LoginResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.token);

        return client;
    }
}