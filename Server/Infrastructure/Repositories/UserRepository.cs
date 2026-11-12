using Infrastructure.DataAccess;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserDatabase _userDb;

    public UserRepository(UserDatabase userDb)
    {
        _userDb = userDb;
    }
    public User? GetUserByUsername(string username)
    {
        return _userDb.Users.Where(x => x.Username == username)
            .FirstOrDefault();

    }

    public bool CreateUser(User user)
    {
        if (_userDb.Users.Any(x => x.Username == user.Username))
        {
            return false;
        }
        _userDb.Users.Add(user);
        _userDb.SaveChanges();
        return true;
    }

    public User? GetById(int id) => _userDb.Users.Find(id);

    public void DeleteUser(User user)
    {
        _userDb.Users.Remove(user);
        _userDb.SaveChanges();
    }
    public List<User> GetAll()
    {
        return _userDb.Users.ToList();
    }
    
    public void UpdateUser(User user)
    {
        _userDb.Users.Update(user);
        _userDb.SaveChanges();
    }
}