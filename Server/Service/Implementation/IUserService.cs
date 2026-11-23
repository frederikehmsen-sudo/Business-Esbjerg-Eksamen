using Infrastructure.Entities;
using Service.DTOs;

namespace Service.Implementation;

public interface IUserService
{
    List<UserResponse> GetUsers(Role role);
    UserResponse CreateUser(Role actorRole, CreateUserRequest request);
    void DeleteUser(Role actorRole, Guid actorId, Guid targetId);
    UserResponse UpdateUser(Role actorRole, Guid actorId, Guid targetId, UpdateUserRequest request);
}