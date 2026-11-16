using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class User
{ 
    [PrimaryKey] public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; }
}