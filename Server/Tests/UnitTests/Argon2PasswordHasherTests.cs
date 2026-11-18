using Service.Security;

namespace Tests.UnitTests;

public class Argon2PasswordHasherTests
{
    private const string Password = "correct-password";

    private readonly Argon2PasswordHasher _hasher = new();

    [Theory]
    [InlineData(Password, true)]
    [InlineData("wrong-password", false)]
    public void VerifyPassword_ReturnsExpectedResult(string attempt, bool expected)
    {
        var hashedPassword = _hasher.HashAndSaltPassword(Password);

        var result = _hasher.VerifyPassword(attempt, hashedPassword);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void HashAndSaltPassword_SamePassword_ProducesDifferentHashes()
    {
        var firstHash = _hasher.HashAndSaltPassword(Password);
        var secondHash = _hasher.HashAndSaltPassword(Password);

        Assert.NotEqual(firstHash, secondHash);
        Assert.True(_hasher.VerifyPassword(Password, firstHash));
        Assert.True(_hasher.VerifyPassword(Password, secondHash));
    }
}