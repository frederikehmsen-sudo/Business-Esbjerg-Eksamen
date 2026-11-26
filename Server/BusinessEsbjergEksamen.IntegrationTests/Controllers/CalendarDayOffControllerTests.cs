namespace BusinessEsbjergEksamen.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using BusinessEsbjergEksamen.IntegrationTests.Fixtures;
using Infrastructure.Entities;
using Service.DTOs;

public class CalendarDayOffControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private static readonly int ThisYear = DateTime.Today.Year;

    [Fact]
    public async Task GenerateDanishHolidays_Twice_DoesNotCreateDuplicates()
    {
        var client = await factory.CreateAdminClientAsync();
        var year = ThisYear + 2;

        var first = await client.PostAsync($"/api/calendardayoff/generate/{year}", null);
        var firstDays = await first.Content.ReadFromJsonAsync<List<CalendarDayOffResponse>>();

        var second = await client.PostAsync($"/api/calendardayoff/generate/{year}", null);
        var secondDays = await second.Content.ReadFromJsonAsync<List<CalendarDayOffResponse>>();

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(12, firstDays!.Count);
        Assert.Empty(secondDays!);
    }

    [Fact]
    public async Task Create_SameDateTwice_ReturnsConflict()
    {
        var client = await factory.CreateAdminClientAsync();
        var request = new CreateCalendarDayOffRequest
        {
            Date = new DateOnly(ThisYear + 3, 6, 15),
            Name = "Firmadag",
            Type = DayOffType.CompanyDayOff
        };

        var first = await client.PostAsJsonAsync("/api/calendardayoff", request);
        var second = await client.PostAsJsonAsync("/api/calendardayoff", request);

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }
}