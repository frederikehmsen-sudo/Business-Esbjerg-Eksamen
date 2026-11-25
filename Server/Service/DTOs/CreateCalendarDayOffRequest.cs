using System.ComponentModel.DataAnnotations;
using Infrastructure.Entities;

namespace Service.DTOs;

public class CreateCalendarDayOffRequest
{
    [Required]
    public DateOnly? Date { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(DayOffType))]
    public DayOffType Type { get; set; }
}