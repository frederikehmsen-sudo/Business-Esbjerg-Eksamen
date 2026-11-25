using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Implementation;

public interface ICalendarDayOffService
{
    List<CalendarDayOffResponse> GetRange(DateOnly from, DateOnly to);
    CalendarDayOffResponse Create(Role actorRole, CreateCalendarDayOffRequest request);
    List<CalendarDayOffResponse> GenerateDanishHolidays(Role actorRole, int year);
    void Delete(Role actorRole, Guid id);
}