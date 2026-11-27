using Microsoft.AspNetCore.Authorization;
using Service.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service.Implementation;

namespace API.Controllers;

[ApiController]
public class LoginController(IAuthService authService, IUserService userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("/login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var result = authService.Login(request);
        return result == null ? Unauthorized() : Ok(result);
    }
}