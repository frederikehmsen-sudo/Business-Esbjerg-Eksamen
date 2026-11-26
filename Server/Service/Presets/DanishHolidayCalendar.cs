using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Presets;


public static class DanishHolidayCalendar
{
    private const int LastYearWithStoreBededag = 2023;

    public static List<CreateCalendarDayOffRequest> ForYear(int year)
    {
        var easter = EasterSunday(year);

        var days = new List<CreateCalendarDayOffRequest>
        {
            Public(new DateOnly(year, 1, 1), "Nytårsdag"),
            Public(easter.AddDays(-3), "Skærtorsdag"),
            Public(easter.AddDays(-2), "Langfredag"),
            Public(easter, "Påskedag"),
            Public(easter.AddDays(1), "2. påskedag"),
        };

        if (year <= LastYearWithStoreBededag)
            days.Add(Public(easter.AddDays(26), "Store bededag"));

        days.AddRange(
        [
            Public(easter.AddDays(39), "Kristi himmelfartsdag"),
            Public(easter.AddDays(49), "Pinsedag"),
            Public(easter.AddDays(50), "2. pinsedag"),
            Company(new DateOnly(year, 12, 24), "Juleaftensdag"),
            Public(new DateOnly(year, 12, 25), "Juledag"),
            Public(new DateOnly(year, 12, 26), "2. juledag"),
            Company(new DateOnly(year, 12, 31), "Nytårsaftensdag")
        ]);

        return days.OrderBy(d => d.Date).ToList();
    }
    
    public static DateOnly EasterSunday(int year)
    {
        var a = year % 19;
        var b = year / 100;
        var c = year % 100;
        var d = b / 4;
        var e = b % 4;
        var f = (b + 8) / 25;
        var g = (b - f + 1) / 3;
        var h = (19 * a + b - d - g + 15) % 30;
        var i = c / 4;
        var k = c % 4;
        var l = (32 + 2 * e + 2 * i - h - k) % 7;
        var m = (a + 11 * h + 22 * l) / 451;
        var month = (h + l - 7 * m + 114) / 31;
        var day = (h + l - 7 * m + 114) % 31 + 1;
        return new DateOnly(year, month, day);
    }

    private static CreateCalendarDayOffRequest Public(DateOnly date, string name) =>
        new() { Date = date, Name = name, Type = DayOffType.PublicHoliday };

    private static CreateCalendarDayOffRequest Company(DateOnly date, string name) =>
        new() { Date = date, Name = name, Type = DayOffType.CompanyDayOff };
}