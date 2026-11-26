using LinqToDB.Mapping;

namespace Infrastructure.Entities;
public class HolidayRuleSet
{
    // Meta
    [PrimaryKey] public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateOnly EffectiveFrom { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }

    //Alt det følgende passer til den nuværende lovgivning
    // Weeks 1–5
    public decimal Weeks1To5DaysEarnedPerMonth { get; set; }               // 2.08 
    public int Weeks1To5EarningStartMonth { get; set; }                    // 9  (1 Sep)
    public int Weeks1To5EarningLengthMonths { get; set; }                  // 12 (→ 31 Aug)
    public int Weeks1To5UsageStartsMonthsAfterEarningStart { get; set; }   // 0  (1 Sep)
    public int Weeks1To5UsageLengthMonths { get; set; }                    // 16 (→ 31 Dec next year)

    // Week 6
    public bool Week6Enabled { get; set; }                                 // true
    public decimal Week6HoursEarnedPerMonth { get; set; }                  // 3.083
    public int Week6EarningStartMonth { get; set; }                        // 1  (1 Jan)
    public int Week6EarningLengthMonths { get; set; }                      // 12 (→ 31 Dec)
    public int Week6UsageStartsMonthsAfterEarningStart { get; set; }       // 16 (1 May next year)
    public int Week6UsageLengthMonths { get; set; }                        // 12 (→ 30 Apr year after)

    // Main holiday
    public int MainHolidayDays { get; set; }                               // 15
    public int MainHolidayWindowStartMonth { get; set; }                   // 5  (1 May)
    public int MainHolidayWindowEndMonth { get; set; }                     // 9  (30 Sep)

    // Notice
    public int NoticeMainHolidayMonths { get; set; }                       // 3
    public int NoticeOtherHolidayMonths { get; set; }                      // 1

    // Transfer
    public int MaxTransferDays { get; set; }                               // 5
    public int Weeks1To5TransferDeadlineMonth { get; set; }                // 12 (31 Dec)
    public int Week6TransferDeadlineMonth { get; set; }                    // 9  (30 Sep)

    // Holiday debt
    public int MaxHolidayDebtDays { get; set; }                            // 5
    public int DebtClearedByMonth { get; set; }                            // 8  (31 Aug)

    // Requests
    public RequestUnit RequestUnit { get; set; }                           // WholeDays
}