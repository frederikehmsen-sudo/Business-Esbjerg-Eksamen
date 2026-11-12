using Infrastructure.Entities;
using LinqToDB;
using LinqToDB.Data;

namespace Infrastructure.DataAccess;

public class UserDatabase : DataConnection
{
  public UserDatabase(DataOptions<UserDatabase> options) : base(options.Options)
  {
  }

  public ITable<User> Users => this.GetTable<User>();
}