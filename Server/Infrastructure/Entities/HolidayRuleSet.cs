using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class HolidayRuleSet
{
    [PrimaryKey] public Guid Id { get; set; }
    public DateOnly EffectiveFrom { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Decimal Weeks1To5DaysPerMonth { get; set; }
    public bool Week6Enabled { get; set; }
    public decimal Week6HoursPerMonth { get; set; }
}