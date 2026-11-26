using System.ComponentModel.DataAnnotations;
using Infrastructure.Entities;

namespace Service.DTOs;

public class CreateEmployeeRequest
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    [EnumDataType(typeof(EmploymentType))]
    public EmploymentType EmploymentType { get; set; }

    [Required, StringLength(128, MinimumLength = 8)]
    public string InitialPassword { get; set; } = string.Empty;

    [Required]
    public WeeklyHoursRequest? WorkSchedule { get; set; }
}

public class UpdateEmployeeRequest
{
    [Required, StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [EnumDataType(typeof(EmploymentType))]
    public EmploymentType EmploymentType { get; set; }
}

public class ResetPasswordRequest
{
    [Required, StringLength(128, MinimumLength = 8)]
    public string NewPassword { get; set; } = string.Empty;
}