using Infrastructure.Entities;
using Infrastructure.Repositories;
using NSubstitute;
using Service.DTOs;
using Service.Implementation;
using Service.Security;
using Service.Security.Auth;

namespace Tests.UnitTests;

public class LoginTests
{
    private const string Username = "Benning";
    private const string Password = "benning";
    private const string StoredHash = "stored-hash";
    private const string Token = "test-token";

    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly AuthService _authService;

    public LoginTests()
    {
        var user = new User
        {
            Username = Username,
            Password = StoredHash,
            Role = Role.Accountant
        };

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.GetUserByUsername(Username).Returns(user);

        var passwordHasher = Substitute.For<IPasswordHasher>();
        passwordHasher.VerifyPassword(Password, StoredHash).Returns(true);

        _tokenService.GenerateToken(user).Returns(Token);

        _authService = new AuthService(userRepository, passwordHasher, _tokenService);
    }

    [Fact]
    public void Login_ValidCredentials_ReturnsTokenAndRole()
    {
        var request = new LoginRequest { Username = Username, Password = Password };

        var result = _authService.Login(request);

        Assert.NotNull(result);
        Assert.Equal(Token, result.token);
        Assert.Equal("Accountant", result.role);
    }

    [Theory]
    [InlineData("unknown-user", Password)]
    [InlineData(Username, "wrong-password")]
    [InlineData("", Password)]
    [InlineData(Username, "")]
    public void Login_InvalidCredentials_ReturnsNull(string username, string password)
    {
        var request = new LoginRequest { Username = username, Password = password };

        var result = _authService.Login(request);

        Assert.Null(result);
        _tokenService.DidNotReceive().GenerateToken(Arg.Any<User>());
    }
}