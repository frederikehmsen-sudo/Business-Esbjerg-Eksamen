using Infrastructure.Entities;

namespace Service.Implementation;

public interface IUserService
{
    List<UserResponse> getUsers();
    UserResponse CreateUser(Role actorRole, createUserRequest request);
    void DeleteUser(Role actorRole, int actorI, int targetId);
    UserResponse UpdateUser(Role actorRole, int actorId, int targetId, UpdateUserRequest request);
}