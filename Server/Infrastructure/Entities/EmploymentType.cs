using System.Text.Json.Serialization;

namespace Infrastructure.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmploymentType
{
    PartTime = 0,
    FullTime = 1,
}