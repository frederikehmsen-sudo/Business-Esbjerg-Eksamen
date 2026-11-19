namespace BusinessEsbjergEksamen.IntegrationTests.Fixtures;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString =
        $"Data Source=TestDb_{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
    
    private readonly SqliteConnection _keepAliveConnection;

    public const string TestJwtSecret = "integration-test-secret-that-is-at-least-32-chars";

    public CustomWebApplicationFactory()
    {
        _keepAliveConnection = new SqliteConnection(_connectionString);
        _keepAliveConnection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Default", _connectionString);
        
        builder.UseSetting("JwtSecret", TestJwtSecret);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _keepAliveConnection.Close();
        _keepAliveConnection.Dispose();
    }
}