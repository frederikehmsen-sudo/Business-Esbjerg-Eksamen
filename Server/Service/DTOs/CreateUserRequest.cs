using System.ComponentModel.DataAnnotations;
using Infrastructure.Entities;

namespace Service.DTOs;

public class CreateUserRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$",
        ErrorMessage = "Username may only contain letters, numbers, '.', '_' and '-'.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [StringLength(128, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(Role))]
    public Role Role { get; set; }
}