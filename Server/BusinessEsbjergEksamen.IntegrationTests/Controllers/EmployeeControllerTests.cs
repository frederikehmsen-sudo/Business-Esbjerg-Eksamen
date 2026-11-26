namespace BusinessEsbjergEksamen.IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using BusinessEsbjergEksamen.IntegrationTests.Fixtures;
using Infrastructure.Entities;
using Service.DTOs;

public class EmployeeControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task Create_AsAdmin_ReturnsCreatedAndEmployeeIsInList()
    {
        var client = await factory.CreateAdminClientAsync();
        var request = NewEmployee();

        var response = await client.PostAsJsonAsync("/api/employee", request);
        var employees = await client.GetFromJsonAsync<List<EmployeeResponse>>("/api/employee");

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Contains(employees!, e => e.Email == request.Email);
    }

    [Fact]
    public async Task Create_AsAccountant_ReturnsForbidden()
    {
        var client = await factory.CreateAccountantClientAsync();

        var response = await client.PostAsJsonAsync("/api/employee", NewEmployee());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task EndEmployment_AsAdmin_SavesEndDate()
    {
        // Arrange
        var client = await factory.CreateAdminClientAsync();
        var createResponse = await client.PostAsJsonAsync("/api/employee", NewEmployee());
        var created = await createResponse.Content.ReadFromJsonAsync<EmployeeResponse>();

        // Act
        var endResponse = await client.DeleteAsync($"/api/employee/{created!.Id}?endDate=2026-12-31");
        var employee = await client.GetFromJsonAsync<EmployeeResponse>($"/api/employee/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, endResponse.StatusCode);
        Assert.Equal(new DateOnly(2026, 12, 31), employee!.EndDate);
    }
    
    private static CreateEmployeeRequest NewEmployee() => new()
    {
        Name = "Test Employee",
        Email = $"test-{Guid.NewGuid():N}@businessesbjerg.com",
        StartDate = new DateOnly(2026, 1, 1),
        EmploymentType = EmploymentType.FullTime,
        InitialPassword = "password123",
        WorkSchedule = new WeeklyHoursRequest
        {
            MondayFrom = new TimeOnly(8, 0),
            MondayTo = new TimeOnly(16, 0)
        }
    };
}