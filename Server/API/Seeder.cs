using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;

namespace API;

public class Seeder(UserDatabase db)
{
    public void Seed()
    {
        db.CreateTable<User>(tableOptions: TableOptions.CreateIfNotExists);
        if (db.Users.Count() == 0)
        {
            db.Insert(new User { Id = 1, Username = "Bennington", Password = "ben", Role = Role.Admin});
        }
    }
}