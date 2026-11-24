using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDatabase _appDb;

    public UserRepository(AppDatabase appDb)
    {
        _appDb = appDb;
    }

    public User? GetUserByUsername(string username) =>
        _appDb.Users.FirstOrDefault(x => x.Username == username);

    public bool CreateUser(User user)
    {
        if (_appDb.Users.Any(x => x.Username == user.Username))
            return false;

        
        _appDb.Insert(user);
        return true;
    }

    public User? GetById(Guid id) =>
        _appDb.Users.FirstOrDefault(x => x.Id == id);

    public void DeleteUser(User user) =>
        _appDb.Delete(user);

    public List<User> GetAll() =>
        _appDb.Users.ToList();

    public void UpdateUser(User user) =>
        _appDb.Update(user);
}