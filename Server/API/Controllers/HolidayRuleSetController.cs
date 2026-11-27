using System.Security.Claims;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Implementation;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HolidayRuleSetController(IHolidayRuleSetService holidayRuleSetService) : ControllerBase
{
    private Role ActorRole =>
        Enum.Parse<Role>(User.FindFirstValue(ClaimTypes.Role)!);

    private Guid ActorId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    [Authorize]
    [HttpGet]
    public List<HolidayRuleSetResponse> GetHistory()
    {
        return holidayRuleSetService.GetHistory();
    }
    
    [Authorize]
    [HttpGet("in-force")]
    [ProducesResponseType(typeof(HolidayRuleSetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult GetInForce([FromQuery] DateOnly date)
    {
        try
        {
            return Ok(holidayRuleSetService.GetInForce(date));
        }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("presets")]
    public List<CreateHolidayRuleSetRequest> GetPresets()
    {
        return holidayRuleSetService.GetPresets();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(HolidayRuleSetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateHolidayRuleSetRequest request)
    {
        try
        {
            var created = holidayRuleSetService.Create(ActorRole, ActorId, request);
            return StatusCode(StatusCodes.Status201Created, created);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }
}