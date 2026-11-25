using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public interface ICalendarDayOffRepository
{
    void Add(CalendarDayOff dayOff);
    void Delete(Guid id);
    CalendarDayOff? GetById(Guid id);
    bool ExistsOnDate(DateOnly date);
    List<CalendarDayOff> GetRange(DateOnly from, DateOnly to);
}