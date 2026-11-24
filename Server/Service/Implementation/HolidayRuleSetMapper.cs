using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Implementation;

public static class HolidayRuleSetMapper
{
    public static HolidayRuleSet ToEntity(CreateHolidayRuleSetRequest r, Guid actorId, DateTime createdAt) => new()
    {
        Id = Guid.NewGuid(),
        Name = r.Name.Trim(),
        EffectiveFrom = r.EffectiveFrom!.Value,
        CreatedAt = createdAt,
        CreatedBy = actorId,

        Weeks1To5DaysEarnedPerMonth = r.Weeks1To5DaysEarnedPerMonth,
        Weeks1To5EarningStartMonth = r.Weeks1To5EarningStartMonth,
        Weeks1To5EarningLengthMonths = r.Weeks1To5EarningLengthMonths,
        Weeks1To5UsageStartsMonthsAfterEarningStart = r.Weeks1To5UsageStartsMonthsAfterEarningStart,
        Weeks1To5UsageLengthMonths = r.Weeks1To5UsageLengthMonths,

        Week6Enabled = r.Week6Enabled,
        Week6HoursEarnedPerMonth = r.Week6Enabled ? r.Week6HoursEarnedPerMonth : 0m,
        Week6EarningStartMonth = r.Week6EarningStartMonth,
        Week6EarningLengthMonths = r.Week6EarningLengthMonths,
        Week6UsageStartsMonthsAfterEarningStart = r.Week6UsageStartsMonthsAfterEarningStart,
        Week6UsageLengthMonths = r.Week6UsageLengthMonths,

        MainHolidayDays = r.MainHolidayDays,
        MainHolidayWindowStartMonth = r.MainHolidayWindowStartMonth,
        MainHolidayWindowEndMonth = r.MainHolidayWindowEndMonth,

        NoticeMainHolidayMonths = r.NoticeMainHolidayMonths,
        NoticeOtherHolidayMonths = r.NoticeOtherHolidayMonths,

        MaxTransferDays = r.MaxTransferDays,
        Weeks1To5TransferDeadlineMonth = r.Weeks1To5TransferDeadlineMonth,
        Week6TransferDeadlineMonth = r.Week6TransferDeadlineMonth,

        MaxHolidayDebtDays = r.MaxHolidayDebtDays,
        DebtClearedByMonth = r.DebtClearedByMonth,

        RequestUnit = r.RequestUnit
    };

    public static HolidayRuleSetResponse ToResponse(HolidayRuleSet h) => new()
    {
        Id = h.Id,
        Name = h.Name,
        EffectiveFrom = h.EffectiveFrom,
        CreatedAt = h.CreatedAt,
        CreatedBy = h.CreatedBy,

        Weeks1To5DaysEarnedPerMonth = h.Weeks1To5DaysEarnedPerMonth,
        Weeks1To5EarningStartMonth = h.Weeks1To5EarningStartMonth,
        Weeks1To5EarningLengthMonths = h.Weeks1To5EarningLengthMonths,
        Weeks1To5UsageStartsMonthsAfterEarningStart = h.Weeks1To5UsageStartsMonthsAfterEarningStart,
        Weeks1To5UsageLengthMonths = h.Weeks1To5UsageLengthMonths,

        Week6Enabled = h.Week6Enabled,
        Week6HoursEarnedPerMonth = h.Week6HoursEarnedPerMonth,
        Week6EarningStartMonth = h.Week6EarningStartMonth,
        Week6EarningLengthMonths = h.Week6EarningLengthMonths,
        Week6UsageStartsMonthsAfterEarningStart = h.Week6UsageStartsMonthsAfterEarningStart,
        Week6UsageLengthMonths = h.Week6UsageLengthMonths,

        MainHolidayDays = h.MainHolidayDays,
        MainHolidayWindowStartMonth = h.MainHolidayWindowStartMonth,
        MainHolidayWindowEndMonth = h.MainHolidayWindowEndMonth,

        NoticeMainHolidayMonths = h.NoticeMainHolidayMonths,
        NoticeOtherHolidayMonths = h.NoticeOtherHolidayMonths,

        MaxTransferDays = h.MaxTransferDays,
        Weeks1To5TransferDeadlineMonth = h.Weeks1To5TransferDeadlineMonth,
        Week6TransferDeadlineMonth = h.Week6TransferDeadlineMonth,

        MaxHolidayDebtDays = h.MaxHolidayDebtDays,
        DebtClearedByMonth = h.DebtClearedByMonth,

        RequestUnit = h.RequestUnit.ToString()
    };
}