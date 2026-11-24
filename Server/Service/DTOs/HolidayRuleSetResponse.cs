using Infrastructure.Entities;

namespace Service.DTOs;

public class HolidayRuleSetResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public DateOnly EffectiveFrom { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid CreatedBy { get; init; }

    public decimal Weeks1To5DaysEarnedPerMonth { get; init; }
    public int Weeks1To5EarningStartMonth { get; init; }
    public int Weeks1To5EarningLengthMonths { get; init; }
    public int Weeks1To5UsageStartsMonthsAfterEarningStart { get; init; }
    public int Weeks1To5UsageLengthMonths { get; init; }

    public bool Week6Enabled { get; init; }
    public decimal Week6HoursEarnedPerMonth { get; init; }
    public int Week6EarningStartMonth { get; init; }
    public int Week6EarningLengthMonths { get; init; }
    public int Week6UsageStartsMonthsAfterEarningStart { get; init; }
    public int Week6UsageLengthMonths { get; init; }

    public int MainHolidayDays { get; init; }
    public int MainHolidayWindowStartMonth { get; init; }
    public int MainHolidayWindowEndMonth { get; init; }

    public int NoticeMainHolidayMonths { get; init; }
    public int NoticeOtherHolidayMonths { get; init; }

    public int MaxTransferDays { get; init; }
    public int Weeks1To5TransferDeadlineMonth { get; init; }
    public int Week6TransferDeadlineMonth { get; init; }

    public int MaxHolidayDebtDays { get; init; }
    public int DebtClearedByMonth { get; init; }

    public string RequestUnit { get; init; } = string.Empty;
}