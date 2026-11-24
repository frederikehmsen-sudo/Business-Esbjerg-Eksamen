namespace Service.DTOs;

public record EmployeeResponse(
    Guid Id, string Name, string Email,
    DateOnly StartDate, DateOnly? EndDate,
    string EmploymentType, bool IsActive);