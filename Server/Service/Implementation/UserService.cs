using Infrastructure.Entities;
using Infrastructure.Repositories;
using Service.DTOs;
using Service.Security;

namespace Service.Implementation;

public class UserService(
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IPasswordHasher passwordHasher) : IUserService
{
    public List<UserResponse> GetUsers(Role role)
    {
        if (!CanSeeList(role))
            throw new UnauthorizedAccessException("You are not allowed to see the user list.");

        return userRepository.GetAll()
            .Select(u => new UserResponse(u.Id, u.Username, u.Role.ToString()))
            .ToList();
    }

    private static bool CanSeeList(Role actor) => actor == Role.Admin;

    public UserResponse CreateUser(Role actorRole, CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Username and password are required.");

        if (!CanCreate(actorRole, request.Role))
            throw new UnauthorizedAccessException("You are not allowed to create this role.");
        
        if (IsSingleRole(request.Role) && RoleTaken(request.Role))
            throw new InvalidOperationException($"There can only be one {request.Role} account.");

        var user = new User
        {
            Username = request.Username,
            Password = passwordHasher.HashAndSaltPassword(request.Password),
            Role = request.Role
        };

        if (!userRepository.CreateUser(user))
            throw new InvalidOperationException("Username already exists.");

        return new UserResponse(user.Id, user.Username, user.Role.ToString());
    }

    private static bool CanCreate(Role actor, Role target) => actor == Role.Admin;

    public void DeleteUser(Role actorRole, Guid actorId, Guid targetId)
    {
        var target = userRepository.GetById(targetId)
                     ?? throw new KeyNotFoundException("User not found.");

        if (target.Id == actorId)
            throw new InvalidOperationException("You cannot delete yourself.");

        if (!CanDelete(actorRole, target.Role))
            throw new UnauthorizedAccessException("You are not allowed to delete this role.");

        if (IsSingleRole(target.Role))
            throw new InvalidOperationException($"You cannot delete the only {target.Role} account.");
        
        if (employeeRepository.GetByUserId(target.Id) is not null)
            throw new InvalidOperationException("This user belongs to an employee. End the employment instead.");

        userRepository.DeleteUser(target);
    }

    private static bool CanDelete(Role actor, Role target) => actor == Role.Admin;

    public UserResponse UpdateUser(Role actorRole, Guid actorId, Guid targetId, UpdateUserRequest request)
    {
        var target = userRepository.GetById(targetId)
                     ?? throw new KeyNotFoundException("User not found.");

        if (string.IsNullOrWhiteSpace(request.Username))
            throw new ArgumentException("Username is required.");

        if (!CanUpdate(actorRole, target.Role))
            throw new UnauthorizedAccessException("You are not allowed to edit this user.");

        var newRole = request.Role ?? target.Role;

        if (!CanUpdate(actorRole, newRole))
            throw new UnauthorizedAccessException("You are not allowed to assign this role.");

        if (target.Id == actorId && newRole != target.Role)
            throw new InvalidOperationException("You cannot change your own role.");

        if (newRole != target.Role)
        {
            if (IsSingleRole(target.Role))
                throw new InvalidOperationException($"You cannot demote the only {target.Role} account.");

            if (IsSingleRole(newRole) && RoleTaken(newRole))
                throw new InvalidOperationException($"There can only be one {newRole} account.");
        }

        var existing = userRepository.GetUserByUsername(request.Username);
        if (existing != null && existing.Id != target.Id)
            throw new InvalidOperationException("Username already exists.");

        target.Username = request.Username;
        target.Role = newRole;

        if (!string.IsNullOrWhiteSpace(request.Password))
            target.Password = passwordHasher.HashAndSaltPassword(request.Password);

        userRepository.UpdateUser(target);

        return new UserResponse(target.Id, target.Username, target.Role.ToString());
    }

    private static bool CanUpdate(Role actor, Role target) => actor == Role.Admin;

    private static bool IsSingleRole(Role role) => role is Role.Admin or Role.Accountant;

    private bool RoleTaken(Role role) => userRepository.GetAll().Any(u => u.Role == role);
}