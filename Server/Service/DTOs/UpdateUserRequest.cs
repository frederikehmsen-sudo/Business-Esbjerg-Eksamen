using System.ComponentModel.DataAnnotations;
using Infrastructure.Entities;

namespace Service.DTOs;

public class UpdateUserRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$")]
    public string Username { get; set; } = string.Empty;
    
    [StringLength(128, MinimumLength = 8)]
    public string? Password { get; set; }

    [EnumDataType(typeof(Role))]
    public Role? Role { get; set; }
}