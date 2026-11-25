using System.Security.Claims;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Implementation;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalendarDayOffController(ICalendarDayOffService calendarDayOffService) : ControllerBase
{
    private Role ActorRole =>
        Enum.Parse<Role>(User.FindFirstValue(ClaimTypes.Role)!);
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(List<CalendarDayOffResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult GetRange([FromQuery] DateOnly from, [FromQuery] DateOnly to)
    {
        try { return Ok(calendarDayOffService.GetRange(from, to)); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(CalendarDayOffResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public IActionResult Create([FromBody] CreateCalendarDayOffRequest request)
    {
        try
        {
            var created = calendarDayOffService.Create(ActorRole, request);
            return StatusCode(StatusCodes.Status201Created, created);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("generate/{year:int}")]
    [ProducesResponseType(typeof(List<CalendarDayOffResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult GenerateDanishHolidays(int year)
    {
        try
        {
            var created = calendarDayOffService.GenerateDanishHolidays(ActorRole, year);
            return StatusCode(StatusCodes.Status201Created, created);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public IActionResult Delete(Guid id)
    {
        try
        {
            calendarDayOffService.Delete(ActorRole, id);
            return NoContent();
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
    }
}