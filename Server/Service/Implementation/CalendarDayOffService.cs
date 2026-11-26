using Infrastructure.Entities;
using Infrastructure.Repositories;
using Service.DTOs;
using Service.Presets;

namespace Service.Implementation;

public class CalendarDayOffService(ICalendarDayOffRepository repository, TimeProvider timeProvider)
    : ICalendarDayOffService
{
    // Roughly two years. Enough for a calendar view or a holiday period, small enough to stay cheap.
    private const int MaxRangeDays = 800;
    private const int MaxYearsAhead = 5;

    public List<CalendarDayOffResponse> GetRange(DateOnly from, DateOnly to)
    {
        if (to < from)
            throw new ArgumentException("'to' must be on or after 'from'.");

        if (to.DayNumber - from.DayNumber > MaxRangeDays)
            throw new ArgumentException($"The range cannot be longer than {MaxRangeDays} days.");

        return repository.GetRange(from, to).Select(CalendarDayOffMapper.ToResponse).ToList();
    }

    public CalendarDayOffResponse Create(Role actorRole, CreateCalendarDayOffRequest request)
    {
        EnsureAdmin(actorRole);
        Validate(request);

        // Days in the past are already "used" by balances and approved holiday. Changing them would rewrite history.
        if (request.Date!.Value < Today())
            throw new InvalidOperationException("Days off in the past cannot be added.");

        if (repository.ExistsOnDate(request.Date.Value))
            throw new InvalidOperationException($"A day off already exists on {request.Date.Value:yyyy-MM-dd}.");

        var entity = CalendarDayOffMapper.ToEntity(request);
        repository.Add(entity);
        return CalendarDayOffMapper.ToResponse(entity);
    }

    public List<CalendarDayOffResponse> GenerateDanishHolidays(Role actorRole, int year)
    {
        EnsureAdmin(actorRole);

        var today = Today();
        if (year < today.Year || year > today.Year + MaxYearsAhead)
            throw new ArgumentException($"Year must be between {today.Year} and {today.Year + MaxYearsAhead}.");

        var created = new List<CalendarDayOffResponse>();

        foreach (var day in DanishHolidayCalendar.ForYear(year))
        {
            // Skip past days (history is locked) and days the admin already defined.
            if (day.Date!.Value < today || repository.ExistsOnDate(day.Date.Value))
                continue;

            var entity = CalendarDayOffMapper.ToEntity(day);
            repository.Add(entity);
            created.Add(CalendarDayOffMapper.ToResponse(entity));
        }

        return created;
    }

    public void Delete(Role actorRole, Guid id)
    {
        EnsureAdmin(actorRole);

        var existing = repository.GetById(id)
                       ?? throw new KeyNotFoundException("Day off not found.");

        if (existing.Date < Today())
            throw new InvalidOperationException("Days off in the past cannot be deleted.");

        repository.Delete(id);
    }

    // --- Helpers ---
    private DateOnly Today() => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

    private static void EnsureAdmin(Role actorRole)
    {
        if (actorRole != Role.Admin)
            throw new UnauthorizedAccessException("Only the system owner can manage days off.");
    }

    private static void Validate(CreateCalendarDayOffRequest r)
    {
        if (r.Date is null)
            throw new ArgumentException("Date is required.");

        if (string.IsNullOrWhiteSpace(r.Name))
            throw new ArgumentException("Name is required.");

        if (r.Name.Trim().Length > 100)
            throw new ArgumentException("Name cannot be longer than 100 characters.");

        if (!Enum.IsDefined(r.Type))
            throw new ArgumentException("Invalid day off type.");
    }
}