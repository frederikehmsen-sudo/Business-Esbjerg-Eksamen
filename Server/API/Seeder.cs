using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;
using Service.Security;

namespace API;

public class Seeder(UserDatabase db, IPasswordHasher passwordHasher)
{
    public void Seed()
    {   
        db.CreateTable<Employee>(tableOptions: TableOptions.CreateIfNotExists);
        db.CreateTable<User>(tableOptions: TableOptions.CreateIfNotExists);
        if (db.Users.Count() == 0)
        {
            db.Insert(new User
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Username = "Bennington",
                Password = passwordHasher.HashAndSaltPassword("ben"),
                Role = Role.Admin
            });
        }
    }
}