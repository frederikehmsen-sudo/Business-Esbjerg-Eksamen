using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Presets;

public static class HolidayRuleSetPresets
{
    public static List<CreateHolidayRuleSetRequest> GetAll() =>
    [
        FerielovenMinimum(),
        FerielovenWith6thWeek()
    ];

    // The Holiday Act only: 5 weeks, no 6th week, no holiday debt
    public static CreateHolidayRuleSetRequest FerielovenMinimum() => new()
    {
        Name = "Ferieloven (minimum)",

        Weeks1To5DaysEarnedPerMonth = 2.08m,
        Weeks1To5EarningStartMonth = 9,
        Weeks1To5EarningLengthMonths = 12,
        Weeks1To5UsageStartsMonthsAfterEarningStart = 0,
        Weeks1To5UsageLengthMonths = 16,

        Week6Enabled = false,
        Week6HoursEarnedPerMonth = 0m,
        Week6EarningStartMonth = 1,
        Week6EarningLengthMonths = 12,
        Week6UsageStartsMonthsAfterEarningStart = 16,
        Week6UsageLengthMonths = 12,

        MainHolidayDays = 15,
        MainHolidayWindowStartMonth = 5,
        MainHolidayWindowEndMonth = 9,

        NoticeMainHolidayMonths = 3,
        NoticeOtherHolidayMonths = 1,

        MaxTransferDays = 5,
        Weeks1To5TransferDeadlineMonth = 12,
        Week6TransferDeadlineMonth = 9,

        MaxHolidayDebtDays = 0,
        DebtClearedByMonth = 8,

        RequestUnit = RequestUnit.WholeDays
    };

    // What Business Esbjerg uses: the Holiday Act + KL 05.11 6th week + 1 week holiday debt
    public static CreateHolidayRuleSetRequest FerielovenWith6thWeek()
    {
        var preset = FerielovenMinimum();
        preset.Name = "Ferieloven + 6. ferieuge";
        preset.Week6Enabled = true;
        preset.Week6HoursEarnedPerMonth = 3.083m;
        preset.MaxHolidayDebtDays = 5;
        return preset;
    }
}