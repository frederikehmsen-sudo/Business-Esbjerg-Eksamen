using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;
using Service.Security;

namespace API;

public class Seeder(UserDatabase db, IPasswordHasher passwordHasher)
{
    public void Seed()
    {
        db.CreateTable<User>(tableOptions: TableOptions.CreateIfNotExists);
        if (db.Users.Count() == 0)
        {
            db.Insert(new User
            {
                Id = 1,
                Username = "Bennington",
                Password = passwordHasher.HashAndSaltPassword("ben"),
                Role = Role.Admin
            });
        }
    }
}