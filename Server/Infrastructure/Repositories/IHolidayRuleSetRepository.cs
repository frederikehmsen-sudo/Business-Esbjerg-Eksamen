using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public interface IHolidayRuleSetRepository
{
    void Add(HolidayRuleSet ruleSet);
    List<HolidayRuleSet> GetAll();
    HolidayRuleSet? GetInForce(DateOnly date);
}