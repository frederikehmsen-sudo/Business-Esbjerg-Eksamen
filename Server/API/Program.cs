using API;
using Infrastructure.DataAccess;
using Infrastructure.Repositories;
using LinqToDB;
using Service;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=db.db";
var options = new DataOptions<UserDatabase>(new DataOptions().UseSQLite(connectionString));
builder.Services.AddScoped<UserDatabase>(_ => new UserDatabase(options));
builder.Services.AddScoped<Seeder>();

var jwtSecret = builder.Configuration["JwtSecret"];
if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret.Length < 32)
{
    throw new InvalidOperationException("JwtSecret must be configured and at least 32 characters");
}

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.ConfigureDomainServices(jwtSecret); // services, JWT auth and SwaggerGen (Swashbuckle)
builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<Seeder>().Seed();
}

app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().SetIsOriginAllowed(_ => true));
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();    // serves /swagger/v1/swagger.json
    app.UseSwaggerUI();  // capital "UI" = Swashbuckle's UI at /swagger
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.MapControllers();

app.Run();