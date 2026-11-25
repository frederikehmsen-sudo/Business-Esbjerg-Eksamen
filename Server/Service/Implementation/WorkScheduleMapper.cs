using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Implementation;

public static class WorkScheduleMapper
{
    public static void Validate(WeeklyHoursRequest hours)
    {
        var days = Days(hours);

        foreach (var (day, from, to) in days)
        {
            if ((from is null) != (to is null))
                throw new ArgumentException($"{day}: set both From and To, or leave both empty.");

            if (from is not null && to!.Value <= from.Value)
                throw new ArgumentException($"{day}: To must be after From.");
        }

        if (days.All(d => d.From is null))
            throw new ArgumentException("The schedule must have at least one working day.");
    }

    public static WorkSchedule ToEntity(WeeklyHoursRequest h, DateOnly validFrom, Guid employeeId = default) => new()
    {
        EmployeeId = employeeId,
        ValidFrom = validFrom,
        ValidTo = null,
        MondayFrom = h.MondayFrom, MondayTo = h.MondayTo,
        TuesdayFrom = h.TuesdayFrom, TuesdayTo = h.TuesdayTo,
        WednesdayFrom = h.WednesdayFrom, WednesdayTo = h.WednesdayTo,
        ThursdayFrom = h.ThursdayFrom, ThursdayTo = h.ThursdayTo,
        FridayFrom = h.FridayFrom, FridayTo = h.FridayTo
    };

    public static WorkScheduleResponse ToResponse(WorkSchedule s)
    {
        var days = Days(s)
            .Select(d => new WorkScheduleDayResponse(d.Day, d.From, d.To, Hours(d.From, d.To)))
            .ToList();

        return new WorkScheduleResponse(s.Id, s.ValidFrom, s.ValidTo, days, days.Sum(d => d.Hours));
    }

    public static decimal Hours(TimeOnly? from, TimeOnly? to)
    {
        if (from is null || to is null) return 0m;
        return Math.Round((decimal)(to.Value - from.Value).TotalHours, 2, MidpointRounding.AwayFromZero);
    }

    private static List<(string Day, TimeOnly? From, TimeOnly? To)> Days(WeeklyHoursRequest h) =>
    [
        ("Monday", h.MondayFrom, h.MondayTo),
        ("Tuesday", h.TuesdayFrom, h.TuesdayTo),
        ("Wednesday", h.WednesdayFrom, h.WednesdayTo),
        ("Thursday", h.ThursdayFrom, h.ThursdayTo),
        ("Friday", h.FridayFrom, h.FridayTo)
    ];

    private static List<(string Day, TimeOnly? From, TimeOnly? To)> Days(WorkSchedule s) =>
    [
        ("Monday", s.MondayFrom, s.MondayTo),
        ("Tuesday", s.TuesdayFrom, s.TuesdayTo),
        ("Wednesday", s.WednesdayFrom, s.WednesdayTo),
        ("Thursday", s.ThursdayFrom, s.ThursdayTo),
        ("Friday", s.FridayFrom, s.FridayTo)
    ];
}