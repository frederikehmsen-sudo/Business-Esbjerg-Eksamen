using API;
using Infrastructure.DataAccess;
using Infrastructure.Repositories;
using LinqToDB;
using Service.Implementation;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default") ?? "Data Source=db.db";
var options = new DataOptions<UserDatabase>(new DataOptions().UseSQLite(connectionString));
builder.Services.AddScoped<UserDatabase>(_ => new UserDatabase(options));
builder.Services.AddScoped<Seeder>();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddOpenApiDocument();
builder.Services.AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<Seeder>().Seed();
}
app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().SetIsOriginAllowed(_ => true));
app.UseOpenApi();
app.UseSwaggerUi();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
