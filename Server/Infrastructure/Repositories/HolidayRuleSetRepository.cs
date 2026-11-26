using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;

namespace Infrastructure.Repositories;

public class HolidayRuleSetRepository : IHolidayRuleSetRepository
{
    private readonly AppDatabase _db;

    public HolidayRuleSetRepository(AppDatabase db)
    {
        _db = db;
    }
    
    public void Add(HolidayRuleSet ruleSet) =>
        _db.Insert(ruleSet);
    
    public List<HolidayRuleSet> GetAll() =>
        _db.HolidayRuleSets
            .OrderByDescending(r => r.EffectiveFrom)
            .ThenByDescending(r => r.CreatedAt)
            .ToList();
    
    public HolidayRuleSet? GetInForce(DateOnly date) =>
        _db.HolidayRuleSets
            .Where(r => r.EffectiveFrom <= date)
            .OrderByDescending(r => r.EffectiveFrom)
            .ThenByDescending(r => r.CreatedAt)
            .FirstOrDefault();
}