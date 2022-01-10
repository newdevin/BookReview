using System.ComponentModel.DataAnnotations;

namespace Auth.WebApi.Dtos
{
    public class VerifyEmailDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
