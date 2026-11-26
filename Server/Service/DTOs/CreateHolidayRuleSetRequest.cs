using System.ComponentModel.DataAnnotations;
using Infrastructure.Entities;

namespace Service.DTOs;

public class CreateHolidayRuleSetRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly? EffectiveFrom { get; set; }

    // Weeks 1–5
    [Range(typeof(decimal), "0.01", "10", ParseLimitsInInvariantCulture = true)]
    public decimal Weeks1To5DaysEarnedPerMonth { get; set; }
    [Range(1, 12)] public int Weeks1To5EarningStartMonth { get; set; }
    [Range(1, 36)] public int Weeks1To5EarningLengthMonths { get; set; }
    [Range(0, 36)] public int Weeks1To5UsageStartsMonthsAfterEarningStart { get; set; }
    [Range(1, 36)] public int Weeks1To5UsageLengthMonths { get; set; }

    // Week 6
    public bool Week6Enabled { get; set; }
    [Range(typeof(decimal), "0", "40", ParseLimitsInInvariantCulture = true)]
    public decimal Week6HoursEarnedPerMonth { get; set; }
    [Range(1, 12)] public int Week6EarningStartMonth { get; set; }
    [Range(1, 36)] public int Week6EarningLengthMonths { get; set; }
    [Range(0, 36)] public int Week6UsageStartsMonthsAfterEarningStart { get; set; }
    [Range(1, 36)] public int Week6UsageLengthMonths { get; set; }

    // Main holiday
    [Range(1, 30)] public int MainHolidayDays { get; set; }
    [Range(1, 12)] public int MainHolidayWindowStartMonth { get; set; }
    [Range(1, 12)] public int MainHolidayWindowEndMonth { get; set; }

    // Notice
    [Range(0, 12)] public int NoticeMainHolidayMonths { get; set; }
    [Range(0, 12)] public int NoticeOtherHolidayMonths { get; set; }

    // Transfer
    [Range(0, 30)] public int MaxTransferDays { get; set; }
    [Range(1, 12)] public int Weeks1To5TransferDeadlineMonth { get; set; }
    [Range(1, 12)] public int Week6TransferDeadlineMonth { get; set; }

    // Holiday debt
    [Range(0, 30)] public int MaxHolidayDebtDays { get; set; }
    [Range(1, 12)] public int DebtClearedByMonth { get; set; }

    // Requests
    [EnumDataType(typeof(RequestUnit))]
    public RequestUnit RequestUnit { get; set; }
}