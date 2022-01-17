using System.ComponentModel.DataAnnotations;

namespace BookReview.WebApi.Dtos;

public class ChangePasswordDto
{
    [Required,EmailAddress]
    public string Email { get; set; }
    [Required]
    public string OriginalPassword { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string RepeatPassword { get; set; }
}
