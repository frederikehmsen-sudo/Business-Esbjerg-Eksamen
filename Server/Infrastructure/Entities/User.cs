using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class User
{ 
    [PrimaryKey, Identity] public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Role Role { get; set; }
}