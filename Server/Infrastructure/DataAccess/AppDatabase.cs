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
  public ITable<Employee> Employees => this.GetTable<Employee>();
  
  public ITable<WorkSchedule> WorkSchedules => this.GetTable<WorkSchedule>();
}
