using System.ComponentModel.DataAnnotations;

namespace Auth.WebApi.Dtos
{
    public class RefreshAccessTokenDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
