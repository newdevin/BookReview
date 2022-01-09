using System.ComponentModel.DataAnnotations;

namespace Auth.WebApi.Dtos;

public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string RepeatPassword { get; set; }

}
