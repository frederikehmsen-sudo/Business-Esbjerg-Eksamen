using System.Text.Json.Serialization;

namespace Infrastructure.Entities;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Employee = 0,
    Accountant = 1,
    Admin = 2   
}