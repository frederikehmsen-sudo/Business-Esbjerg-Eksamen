using Infrastructure.Entities;

namespace Service.Implementation;

public class UserService(IUserRepository userRepository) : IUserService
{
    public List<UserResponse> GetUsers(Role role) =>
        userRepository.GetAll()
            .Where(u =>
                role == Role.Admin ||
                (role == Role.Employee && u.Role == Role.Employee) ||
                (role == Role.Accountant &&
                 (u.Role == Role.Employee || u.Role == Role.Accountant))
            )
            .Select(u => new UserResponse(u.Id, u.Username, u.Role.ToString()))
            .ToList();

    public UserResponse CreateUser(Role actorRole, CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Username and password are required.");

        if (!CanCreate(actorRole, request.Role))
            throw new UnauthorizedAccessException("You are not allowed to create this role.");

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

    public void DeleteUser(Role actorRole, int actorI, int targetId)
    {
        throw new NotImplementedException();
    }

    public UserResponse UpdateUser(Role actorRole, int actorId, int targetId, UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }
}