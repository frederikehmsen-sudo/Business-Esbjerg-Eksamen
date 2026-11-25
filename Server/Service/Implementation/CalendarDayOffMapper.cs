using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Implementation;

public static class CalendarDayOffMapper
{
    public static CalendarDayOff ToEntity(CreateCalendarDayOffRequest r) => new()
    {
        Id = Guid.NewGuid(),
        Date = r.Date!.Value,
        Name = r.Name.Trim(),
        Type = r.Type
    };

    public static CalendarDayOffResponse ToResponse(CalendarDayOff d) => new()
    {
        Id = d.Id,
        Date = d.Date,
        Name = d.Name,
        Type = d.Type.ToString()
    };
}