using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;

namespace Infrastructure.Repositories;

public class CalendarDayOffRepository : ICalendarDayOffRepository
{
    private readonly AppDatabase _db;

    public CalendarDayOffRepository(AppDatabase db)
    {
        _db = db;
    }

    public void Add(CalendarDayOff dayOff) =>
        _db.Insert(dayOff);

    public void Delete(Guid id) =>
        _db.CalendarDayOffs.Where(d => d.Id == id).Delete();

    public CalendarDayOff? GetById(Guid id) =>
        _db.CalendarDayOffs.FirstOrDefault(d => d.Id == id);

    public bool ExistsOnDate(DateOnly date) =>
        _db.CalendarDayOffs.Any(d => d.Date == date);

    public List<CalendarDayOff> GetRange(DateOnly from, DateOnly to) =>
        _db.CalendarDayOffs
            .Where(d => d.Date >= from && d.Date <= to)
            .OrderBy(d => d.Date)
            .ToList();
}