using Infrastructure.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Infrastructure.DataAccess;

public class AppDatabase : DataConnection
{
  public AppDatabase(DataOptions<AppDatabase> options) : base(options.Options)
  {
  }

  public ITable<User> Users => this.GetTable<User>();
  public ITable<HolidayRuleSet> HolidayRuleSets => this.GetTable<HolidayRuleSet>();
  public ITable<CalendarDayOff> CalendarDayOffs => this.GetTable<CalendarDayOff>();
  public ITable<Employee> Employees => this.GetTable<Employee>();
}
