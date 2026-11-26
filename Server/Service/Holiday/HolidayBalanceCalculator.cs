using Infrastructure.Entities;

namespace Service.Holiday;

public record HolidayUsage(DateOnly Date, HolidayAccount Account);

// A "pot" is the holiday earned in one earning period.
public class HolidayPot
{
    public HolidayAccount Account { get; init; }
    public DateOnly EarningFrom { get; init; }
    public DateOnly EarningTo { get; init; }
    public DateOnly UsableFrom { get; init; }
    public DateOnly UsableTo { get; init; } // expiry date

    public decimal EarnedDays { get; internal set; }
    public decimal UsedDays { get; internal set; }
    public decimal RemainingDays => EarnedDays - UsedDays;
    public decimal RemainingHours { get; internal set; }

    internal List<(DateOnly CreditedOn, decimal Days)> Credits { get; } = [];

    public bool IsUsableOn(DateOnly date) => date >= UsableFrom && date <= UsableTo;

    internal decimal EarnedOn(DateOnly date) => Credits.Where(c => c.CreditedOn <= date).Sum(c => c.Days);
}

public record HolidayBalance(DateOnly AsOf, decimal DayLengthHours, IReadOnlyList<HolidayPot> Pots)
{
    public decimal AvailableDays(HolidayAccount account) =>
        Pots.Where(p => p.Account == account && p.IsUsableOn(AsOf)).Sum(p => p.RemainingDays);

    public decimal AvailableHours(HolidayAccount account) =>
        HolidayBalanceCalculator.RoundHours(AvailableDays(account) * DayLengthHours);
}

public class HolidayBalanceCalculator
{
    public const decimal DefaultFullTimeWeeklyHours = 37m;
    private const int WorkDaysPerWeek = 5;

    public HolidayBalance Calculate(
        Employee employee,
        IReadOnlyList<WorkSchedule> schedules,
        IReadOnlyList<HolidayRuleSet> ruleSets,
        IReadOnlyList<HolidayUsage> usages,
        DateOnly asOf,
        decimal fullTimeWeeklyHours = DefaultFullTimeWeeklyHours)
    {
        var pots = new Dictionary<(HolidayAccount, DateOnly), HolidayPot>();

        AddAccrual(employee, ruleSets, asOf, fullTimeWeeklyHours, pots);
        SpendUsages(schedules, usages, asOf, pots);

        var dayLength = DayLengthForDisplay(schedules, asOf);
        foreach (var pot in pots.Values)
        {
            pot.EarnedDays = pot.EarnedOn(asOf);
            pot.RemainingHours = RoundHours(pot.RemainingDays * dayLength);
        }

        var ordered = pots.Values.OrderBy(p => p.Account).ThenBy(p => p.EarningFrom).ToList();
        return new HolidayBalance(asOf, dayLength, ordered);
    }

    public static decimal RoundHours(decimal hours) => Math.Round(hours, 2, MidpointRounding.AwayFromZero);

    // accrual

    private static void AddAccrual(
        Employee employee,
        IReadOnlyList<HolidayRuleSet> ruleSets,
        DateOnly asOf,
        decimal fullTimeWeeklyHours,
        Dictionary<(HolidayAccount, DateOnly), HolidayPot> pots)
    {
        var lastDay = employee.EndDate is { } end && end < asOf ? end : asOf;
        if (lastDay < employee.StartDate)
            return;

        var fullTimeDayLength = fullTimeWeeklyHours / WorkDaysPerWeek;
        var month = new DateOnly(employee.StartDate.Year, employee.StartDate.Month, 1);

        while (month <= lastDay)
        {
            var monthEnd = month.AddMonths(1).AddDays(-1);
            var from = Max(month, employee.StartDate);
            var to = employee.EndDate is { } e ? Min(monthEnd, e) : monthEnd;
            var share = (to.DayNumber - from.DayNumber + 1) / (decimal)monthEnd.Day;

            var rules = RuleSetFor(ruleSets, month);

            var weeks1To5 = GetOrCreatePot(pots, HolidayAccount.Weeks1To5, month,
                rules.Weeks1To5EarningStartMonth, rules.Weeks1To5EarningLengthMonths,
                rules.Weeks1To5UsageStartsMonthsAfterEarningStart, rules.Weeks1To5UsageLengthMonths);
            weeks1To5.Credits.Add((to, rules.Weeks1To5DaysEarnedPerMonth * share));

            if (rules.Week6Enabled)
            {
                var week6 = GetOrCreatePot(pots, HolidayAccount.Week6, month,
                    rules.Week6EarningStartMonth, rules.Week6EarningLengthMonths,
                    rules.Week6UsageStartsMonthsAfterEarningStart, rules.Week6UsageLengthMonths);
                var daysPerMonth = rules.Week6HoursEarnedPerMonth / fullTimeDayLength;
                week6.Credits.Add((to, daysPerMonth * share));
            }

            month = month.AddMonths(1);
        }
    }

    private static HolidayRuleSet RuleSetFor(IReadOnlyList<HolidayRuleSet> ruleSets, DateOnly monthStart) =>
        ruleSets.Where(r => r.EffectiveFrom <= monthStart).MaxBy(r => r.EffectiveFrom)
        ?? throw new InvalidOperationException($"No holiday rule set is in effect for {monthStart:yyyy-MM}.");

    private static HolidayPot GetOrCreatePot(
        Dictionary<(HolidayAccount, DateOnly), HolidayPot> pots,
        HolidayAccount account,
        DateOnly month,
        int earningStartMonth,
        int earningLengthMonths,
        int usageStartsMonthsAfterEarningStart,
        int usageLengthMonths)
    {
        var startYear = month.Month >= earningStartMonth ? month.Year : month.Year - 1;
        var earningFrom = new DateOnly(startYear, earningStartMonth, 1);

        if (pots.TryGetValue((account, earningFrom), out var pot))
            return pot;

        var usableFrom = earningFrom.AddMonths(usageStartsMonthsAfterEarningStart);
        pot = new HolidayPot
        {
            Account = account,
            EarningFrom = earningFrom,
            EarningTo = earningFrom.AddMonths(earningLengthMonths).AddDays(-1),
            UsableFrom = usableFrom,
            UsableTo = usableFrom.AddMonths(usageLengthMonths).AddDays(-1),
        };
        pots[(account, earningFrom)] = pot;
        return pot;
    }

    // usage

    private static void SpendUsages(
        IReadOnlyList<WorkSchedule> schedules,
        IReadOnlyList<HolidayUsage> usages,
        DateOnly asOf,
        Dictionary<(HolidayAccount, DateOnly), HolidayPot> pots)
    {
        foreach (var usage in usages.Where(u => u.Date <= asOf).OrderBy(u => u.Date))
        {
            var cost = CostInDays(schedules, usage.Date);
            if (cost == 0)
                continue;

            var usable = pots.Values
                .Where(p => p.Account == usage.Account && p.IsUsableOn(usage.Date))
                .OrderBy(p => p.UsableTo)
                .ToList();

            if (usable.Count == 0)
                throw new InvalidOperationException(
                    $"No {usage.Account} holiday can be used on {usage.Date:yyyy-MM-dd}.");

            var left = cost;
            foreach (var pot in usable)
            {
                var available = pot.EarnedOn(usage.Date) - pot.UsedDays;
                if (available <= 0)
                    continue;

                var take = Math.Min(available, left);
                pot.UsedDays += take;
                left -= take;
                if (left == 0)
                    break;
            }

            if (left > 0)
                usable[^1].UsedDays += left; 
        }
    }
    
    public static decimal CostInDays(IReadOnlyList<WorkSchedule> schedules, DateOnly date)
    {
        var schedule = ActiveSchedule(schedules, date)
                       ?? throw new InvalidOperationException($"No work schedule on {date:yyyy-MM-dd}.");

        var dayLength = DayLength(schedule);
        return dayLength == 0 ? 0 : WeekdayHours(schedule, date.DayOfWeek) / dayLength;
    }
    
    public static decimal CostInHours(IReadOnlyList<WorkSchedule> schedules, DateOnly date)
    {
        var schedule = ActiveSchedule(schedules, date)
                       ?? throw new InvalidOperationException($"No work schedule on {date:yyyy-MM-dd}.");
        return WeekdayHours(schedule, date.DayOfWeek);
    }

    // schedule helpers

    private static WorkSchedule? ActiveSchedule(IReadOnlyList<WorkSchedule> schedules, DateOnly date) =>
        schedules.FirstOrDefault(s => s.ValidFrom <= date && (s.ValidTo is null || s.ValidTo >= date));
    
    private static decimal DayLengthForDisplay(IReadOnlyList<WorkSchedule> schedules, DateOnly date)
    {
        var schedule = ActiveSchedule(schedules, date)
                       ?? schedules.Where(s => s.ValidFrom <= date).MaxBy(s => s.ValidFrom)
                       ?? schedules.MinBy(s => s.ValidFrom);

        return schedule is null ? 0 : DayLength(schedule);
    }

    public static decimal WeeklyHours(WorkSchedule s) =>
        Hours(s.MondayFrom, s.MondayTo) + Hours(s.TuesdayFrom, s.TuesdayTo) +
        Hours(s.WednesdayFrom, s.WednesdayTo) + Hours(s.ThursdayFrom, s.ThursdayTo) +
        Hours(s.FridayFrom, s.FridayTo);

    public static decimal DayLength(WorkSchedule s) => WeeklyHours(s) / WorkDaysPerWeek;

    private static decimal WeekdayHours(WorkSchedule s, DayOfWeek day) => day switch
    {
        DayOfWeek.Monday => Hours(s.MondayFrom, s.MondayTo),
        DayOfWeek.Tuesday => Hours(s.TuesdayFrom, s.TuesdayTo),
        DayOfWeek.Wednesday => Hours(s.WednesdayFrom, s.WednesdayTo),
        DayOfWeek.Thursday => Hours(s.ThursdayFrom, s.ThursdayTo),
        DayOfWeek.Friday => Hours(s.FridayFrom, s.FridayTo),
        _ => 0m,
    };

    private static decimal Hours(TimeOnly? from, TimeOnly? to) =>
        from is { } f && to is { } t && t > f ? (decimal)(t - f).TotalHours : 0m;

    private static DateOnly Max(DateOnly a, DateOnly b) => a > b ? a : b;
    private static DateOnly Min(DateOnly a, DateOnly b) => a < b ? a : b;
}