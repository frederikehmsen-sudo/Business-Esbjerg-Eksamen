using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Service.Security.Auth;

namespace Tests.UnitTests;

public class JwtAuthorizationServiceTests
{
    private const string Secret = "test-secret-with-at-least-32-characters";
    private const string IssuerAndAudience = "security-api";

    [Fact]
    public void GenerateToken_ValidUser_ReturnsValidTokenWithClaims()
    {
        // Arrange
        var configuration = Substitute.For<IConfiguration>();
        configuration["JwtSecret"].Returns(Secret);

        var service = new JwtAuthorizationService(configuration);
        var user = new User { Role = Role.Admin };
        var createdAt = DateTime.UtcNow;

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = IssuerAndAudience,
            ValidateAudience = true,
            ValidAudience = IssuerAndAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))
        };

        // Act
        var token = service.GenerateToken(user);

        // Assert
        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token, validationParameters, out var validatedToken);

        Assert.Equal(user.Id.ToString(), principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal("Admin", principal.FindFirst(ClaimTypes.Role)?.Value);
        Assert.InRange(validatedToken.ValidTo, createdAt.AddMinutes(59), createdAt.AddMinutes(61));
    }
}