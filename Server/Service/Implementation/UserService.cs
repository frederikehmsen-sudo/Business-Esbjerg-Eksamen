using Infrastructure.Entities;
using Infrastructure.Repositories;
using Service.DTOs;

namespace Service.Implementation;

public class UserService(IUserRepository userRepository) : IUserService
{
    public List<UserResponse> GetUsers(Role role)
    {

        return userRepository.GetAll()
            .Select(u => new UserResponse(u.Id, u.Username, u.Role.ToString()))
            .ToList();
    }

    public UserResponse CreateUser(Role actorRole, CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Username and password are required.");

        var user = new User
        {
            Username = request.Username,
            Password = request.Password,
            Role = request.Role
        };

        if (!userRepository.CreateUser(user))
            throw new InvalidOperationException("Username already exists.");

        return new UserResponse(user.Id, user.Username, user.Role.ToString());
    }
    private static bool CanCreate(Role actor, Role target) => actor switch
    {
        Role.Admin => true,
        Role.Accountant => target == Role.Employee,
        _ => false
    };  

    public void DeleteUser(Role actorRole, int actorId, int targetId)
    {
        var target = userRepository.GetById(targetId)
                     ?? throw new KeyNotFoundException("User not found.");

        if (target.Id == actorId)
            throw new InvalidOperationException("You cannot delete yourself.");
        

        userRepository.DeleteUser(target);
    }
    private static bool CanDelete(Role actor, Role target) => actor switch
    {
        Role.Admin => true,
        Role.Accountant => target == Role.Employee,
        _ => false
    };  

    public UserResponse UpdateUser(Role actorRole, int actorId, int targetId, UpdateUserRequest request)
    {
        var target = userRepository.GetById(targetId)
                     ?? throw new KeyNotFoundException("User not found.");

        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username is required.");

        var newRole = request.Role ?? target.Role;
        
        
        if (target.Id == actorId && newRole != target.Role)
            throw new InvalidOperationException("You cannot change your own role.");

        var existing = userRepository.GetUserByUsername(request.Username);
        if (existing != null && existing.Id != target.Id)
            throw new InvalidOperationException("Username already exists.");

        target.Username = request.Username;
        target.Role = newRole;

        userRepository.UpdateUser(target);

        return new UserResponse(target.Id, target.Username, target.Role.ToString());
    }
    
    private static bool CanUpdate(Role actor, Role target) => actor switch
    {
        Role.Admin => true,
        Role.Accountant => target == Role.Employee,
        _ => false
    };
}