using Infrastructure.Entities;
using Infrastructure.Repositories;
using Service.DTOs;
using Service.Presets;

namespace Service.Implementation;

public class HolidayRuleSetService(IHolidayRuleSetRepository repository, TimeProvider timeProvider)
    : IHolidayRuleSetService
{
    public HolidayRuleSetResponse Create(Role actorRole, Guid actorId, CreateHolidayRuleSetRequest request)
    {
        if (actorRole != Role.Admin)
            throw new UnauthorizedAccessException("Only the system owner can create holiday rule sets.");

        Validate(request);
        
        var ruleSet = HolidayRuleSetMapper.ToEntity(request, actorId, timeProvider.GetUtcNow().UtcDateTime);
        repository.Add(ruleSet);

        return HolidayRuleSetMapper.ToResponse(ruleSet);
    }

    public List<HolidayRuleSetResponse> GetHistory() =>
        repository.GetAll().Select(HolidayRuleSetMapper.ToResponse).ToList();

    public HolidayRuleSetResponse GetInForce(DateOnly date)
    {
        var ruleSet = repository.GetInForce(date)
                      ?? throw new KeyNotFoundException($"No holiday rule set is in force on {date}.");
        return HolidayRuleSetMapper.ToResponse(ruleSet); 
    }

    public List<CreateHolidayRuleSetRequest> GetPresets() =>
        HolidayRuleSetPresets.GetAll();

    // --- Validation ---
    private void Validate(CreateHolidayRuleSetRequest r)
    {
        if (string.IsNullOrWhiteSpace(r.Name))
            throw new ArgumentException("Name is required.");

        if (r.EffectiveFrom is null)
            throw new ArgumentException("EffectiveFrom is required.");

        var today = DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);
        if (r.EffectiveFrom.Value < today)
            throw new ArgumentException("EffectiveFrom cannot be in the past.");

        if (r.Weeks1To5DaysEarnedPerMonth <= 0)
            throw new ArgumentException("Weeks 1–5 days earned per month must be greater than 0.");

        if (r.Week6Enabled && r.Week6HoursEarnedPerMonth <= 0)
            throw new ArgumentException("Week 6 hours earned per month must be greater than 0 when week 6 is enabled.");

        ValidateMonth(r.Weeks1To5EarningStartMonth, "Weeks 1–5 earning start month");
        ValidateMonth(r.Week6EarningStartMonth, "Week 6 earning start month");
        ValidateMonth(r.MainHolidayWindowStartMonth, "Main holiday window start month");
        ValidateMonth(r.MainHolidayWindowEndMonth, "Main holiday window end month");
        ValidateMonth(r.Weeks1To5TransferDeadlineMonth, "Weeks 1–5 transfer deadline month");
        ValidateMonth(r.Week6TransferDeadlineMonth, "Week 6 transfer deadline month");
        ValidateMonth(r.DebtClearedByMonth, "Debt cleared by month");

        if (r.Weeks1To5EarningLengthMonths <= 0 || r.Weeks1To5UsageLengthMonths <= 0 ||
            r.Week6EarningLengthMonths <= 0 || r.Week6UsageLengthMonths <= 0)
            throw new ArgumentException("Period lengths must be greater than 0.");

        if (r.MainHolidayWindowEndMonth < r.MainHolidayWindowStartMonth)
            throw new ArgumentException("Main holiday window must end after it starts.");

        if (r.MainHolidayDays <= 0)
            throw new ArgumentException("Main holiday days must be greater than 0.");

        if (r.NoticeMainHolidayMonths < 0 || r.NoticeOtherHolidayMonths < 0 ||
            r.MaxTransferDays < 0 || r.MaxHolidayDebtDays < 0)
            throw new ArgumentException("Notice, transfer and debt values cannot be negative.");

        if (!Enum.IsDefined(r.RequestUnit))
            throw new ArgumentException("Invalid request unit.");
    }

    private static void ValidateMonth(int month, string field)
    {
        if (month is < 1 or > 12)
            throw new ArgumentException($"{field} must be between 1 and 12.");
    }
}