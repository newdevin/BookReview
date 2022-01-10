using System.ComponentModel.DataAnnotations;

namespace Auth.WebApi.Dtos
{
    public class LogoutDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
