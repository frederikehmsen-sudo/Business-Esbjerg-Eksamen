using Infrastructure.Entities;
using Service.DTOs;
using Service.Holiday;
using Service.Implementation;
using Service.Presets;

namespace Tests.UnitTests;

public class HolidayBalanceCalculatorTests
{
    private readonly HolidayBalanceCalculator _calculator = new();

    [Fact]
    public void Calculate_FullEarningYear_Gives2Point08DaysPerMonth()
    {
        // Arrange
        var employee = NewEmployee(new DateOnly(2025, 9, 1));
        var schedules = new List<WorkSchedule> { FullTime(employee.StartDate) };
        var rules = Rules(HolidayRuleSetPresets.FerielovenMinimum());

        // Act
        var balance = _calculator.Calculate(employee, schedules, rules, [], new DateOnly(2026, 8, 31));

        // Assert
        Assert.Equal(24.96m, balance.AvailableDays(HolidayAccount.Weeks1To5));
    }

    [Fact]
    public void Calculate_StartMidMonth_EarnsOnlyForDaysEmployed()
    {
        // Arrange
        var employee = NewEmployee(new DateOnly(2025, 9, 16));
        var schedules = new List<WorkSchedule> { FullTime(employee.StartDate) };
        var rules = Rules(HolidayRuleSetPresets.FerielovenMinimum());

        // Act
        var balance = _calculator.Calculate(employee, schedules, rules, [], new DateOnly(2025, 9, 30));

        // Assert
        Assert.Equal(1.04m, balance.AvailableDays(HolidayAccount.Weeks1To5));
    }

    [Fact]
    public void Calculate_PartTimeUsage_DeductsTheHoursOfThatWeekday()
    {
        // Arrange
        var employee = NewEmployee(new DateOnly(2025, 9, 1));
        var schedule = new WorkSchedule
        {
            ValidFrom = employee.StartDate,
            MondayFrom = new TimeOnly(8, 0), MondayTo = new TimeOnly(16, 0),
            TuesdayFrom = new TimeOnly(8, 0), TuesdayTo = new TimeOnly(12, 0)
        };
        var rules = Rules(HolidayRuleSetPresets.FerielovenMinimum());
        var mondayOff = new HolidayUsage(new DateOnly(2026, 8, 3), HolidayAccount.Weeks1To5); // a Monday

        // Act
        var balance = _calculator.Calculate(employee, [schedule], rules, [mondayOff], new DateOnly(2026, 8, 31));

        // Assert
        Assert.Equal(51.90m, balance.AvailableHours(HolidayAccount.Weeks1To5));
    }

    [Fact]
    public void Calculate_Week6_CanOnlyBeUsedFromMayTheFollowingYear()
    {
        // Arrange
        var employee = NewEmployee(new DateOnly(2025, 1, 1));
        var schedules = new List<WorkSchedule> { FullTime(employee.StartDate) };
        var rules = Rules(HolidayRuleSetPresets.FerielovenWith6thWeek());

        // Act
        var dayBefore = _calculator.Calculate(employee, schedules, rules, [], new DateOnly(2026, 4, 30));
        var firstDay = _calculator.Calculate(employee, schedules, rules, [], new DateOnly(2026, 5, 1));

        // Assert
        Assert.Equal(0m, dayBefore.AvailableDays(HolidayAccount.Week6));
        Assert.Equal(5.0m, Math.Round(firstDay.AvailableDays(HolidayAccount.Week6), 1));
    }

    // Helpers

    private static Employee NewEmployee(DateOnly start) => new() { StartDate = start };
    
    private static WorkSchedule FullTime(DateOnly validFrom)
    {
        var from = new TimeOnly(8, 0);
        var to = new TimeOnly(15, 24);
        return new WorkSchedule
        {
            ValidFrom = validFrom,
            MondayFrom = from, MondayTo = to,
            TuesdayFrom = from, TuesdayTo = to,
            WednesdayFrom = from, WednesdayTo = to,
            ThursdayFrom = from, ThursdayTo = to,
            FridayFrom = from, FridayTo = to
        };
    }

    private static List<HolidayRuleSet> Rules(CreateHolidayRuleSetRequest preset)
    {
        preset.EffectiveFrom = new DateOnly(2000, 1, 1);
        return [HolidayRuleSetMapper.ToEntity(preset, Guid.Empty, DateTime.MinValue)];
    }
}