using System.Text.Json.Serialization;

namespace Infrastructure.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RequestUnit
{
    WholeDays = 0
}