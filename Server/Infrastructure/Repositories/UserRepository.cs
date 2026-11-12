using Infrastructure.DataAccess;
using Infrastructure.Entities;
using LinqToDB;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDatabase _userDb;

    public UserRepository(UserDatabase userDb)
    {
        _userDb = userDb;
    }

    public User? GetUserByUsername(string username) =>
        _userDb.Users.FirstOrDefault(x => x.Username == username);

    public bool CreateUser(User user)
    {
        if (_userDb.Users.Any(x => x.Username == user.Username))
            return false;

        // Inserts and returns the generated Id, so UserService can return it
        user.Id = _userDb.InsertWithInt32Identity(user);
        return true;
    }

    public User? GetById(int id) =>
        _userDb.Users.FirstOrDefault(x => x.Id == id);

    public void DeleteUser(User user) =>
        _userDb.Delete(user);

    public List<User> GetAll() =>
        _userDb.Users.ToList();

    public void UpdateUser(User user) =>
        _userDb.Update(user);
}