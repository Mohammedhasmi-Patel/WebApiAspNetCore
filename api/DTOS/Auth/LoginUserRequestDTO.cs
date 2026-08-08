using System.ComponentModel.DataAnnotations;

namespace api.DTOS.Auth;

public class LoginUserRequestDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

}
