using System.Security.Claims;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Implementation;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkScheduleController(IWorkScheduleService workScheduleService) : ControllerBase
{
    private Role ActorRole =>
        Enum.Parse<Role>(User.FindFirstValue(ClaimTypes.Role)!);

    private Guid ActorId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    [Authorize]
    [HttpGet("employee/{employeeId:guid}")]
    [ProducesResponseType(typeof(List<WorkScheduleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult GetForEmployee(Guid employeeId)
    {
        try { return Ok(workScheduleService.GetForEmployee(ActorRole, ActorId, employeeId)); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }
    
    [Authorize(Roles = "Employee")]
    [HttpGet("me")]
    [ProducesResponseType(typeof(List<WorkScheduleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult GetMine()
    {
        try { return Ok(workScheduleService.GetMine(ActorId)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }
    
    [Authorize(Roles = "Admin,Accountant")]
    [HttpPost("employee/{employeeId:guid}")]
    [ProducesResponseType(typeof(WorkScheduleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    public IActionResult Add(Guid employeeId, [FromBody] CreateWorkScheduleRequest request)
    {
        try
        {
            var created = workScheduleService.Add(ActorRole, employeeId, request);
            return StatusCode(StatusCodes.Status201Created, created);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }
}