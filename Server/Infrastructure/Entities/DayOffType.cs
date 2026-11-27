using System.Text.Json.Serialization;

namespace Infrastructure.Entities;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DayOffType
{
    PublicHoliday = 0,
    CompanyDayOff = 1
}