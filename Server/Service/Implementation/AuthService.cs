using Infrastructure.Repositories;
using Service.DTOs;
using Service.Security;
using Service.Security.Auth;

namespace Service.Implementation;

public class AuthService(
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    TimeProvider timeProvider) : IAuthService
{
    private DateOnly Today => DateOnly.FromDateTime(timeProvider.GetLocalNow().DateTime);

    public LoginResponse? Login(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            return null;

        var user = userRepository.GetUserByUsername(request.Username);
        if (user == null || !user.IsActive) return null;
        
        // Admin/Accountant have no Employee row, so this only applies to employees.
        var employee = employeeRepository.GetByUserId(user.Id);
        if (employee is not null)
        {
            if (employee.StartDate > Today) return null;
            if (employee.EndDate is { } end && end < Today) return null;
        }

        if (!passwordHasher.VerifyPassword(request.Password, user.Password))
            return null;

        return new LoginResponse(tokenService.GenerateToken(user), user.Role.ToString());
    }
}