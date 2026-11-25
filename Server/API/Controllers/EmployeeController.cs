using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Implementation;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class EmployeeController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet]
    public List<EmployeeResponse> GetAll([FromQuery] bool includeFormer = false)
    {
        return employeeService.GetAll(includeFormer);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        try { return Ok(employeeService.GetById(id)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateEmployeeRequest request)
    {
        try
        {
            var created = employeeService.Create(request);
            return StatusCode(StatusCodes.Status201Created, created);
        }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        try { return Ok(employeeService.Update(id, request)); }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult EndEmployment(Guid id, [FromQuery] DateOnly? endDate)
    {
        try
        {
            employeeService.EndEmployment(id, endDate);
            return NoContent();
        }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (InvalidOperationException e) { return Conflict(e.Message); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }

    [HttpPost("{id:guid}/reset-password")]
    public IActionResult ResetPassword(Guid id, [FromBody] ResetPasswordRequest request)
    {
        try
        {
            employeeService.ResetPassword(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException e) { return NotFound(e.Message); }
        catch (ArgumentException e) { return BadRequest(e.Message); }
    }
}