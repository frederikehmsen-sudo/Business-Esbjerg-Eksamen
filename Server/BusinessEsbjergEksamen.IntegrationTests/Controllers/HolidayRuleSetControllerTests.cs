namespace BusinessEsbjergEksamen.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using BusinessEsbjergEksamen.IntegrationTests.Fixtures;
using Service.DTOs;

public class HolidayRuleSetControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Today);

    [Fact]
    public async Task Create_FromPreset_IsInForceFromItsEffectiveDate()
    {
        // Arrange
        var client = await factory.CreateAdminClientAsync();
        var presets = await client.GetFromJsonAsync<List<CreateHolidayRuleSetRequest>>("/api/holidayruleset/presets");
        var request = presets![0];
        request.Name = "New rules";
        request.EffectiveFrom = Today.AddYears(1);

        // Act
        var createResponse = await client.PostAsJsonAsync("/api/holidayruleset", request);
        var inForce = await client.GetFromJsonAsync<HolidayRuleSetResponse>(
            $"/api/holidayruleset/in-force?date={Today.AddYears(1):yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.Equal("New rules", inForce!.Name);
    }

    [Fact]
    public async Task Create_EffectiveFromInPast_ReturnsBadRequest()
    {
        var client = await factory.CreateAdminClientAsync();
        var presets = await client.GetFromJsonAsync<List<CreateHolidayRuleSetRequest>>("/api/holidayruleset/presets");
        var request = presets![0];
        request.EffectiveFrom = Today.AddDays(-1);

        var response = await client.PostAsJsonAsync("/api/holidayruleset", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}