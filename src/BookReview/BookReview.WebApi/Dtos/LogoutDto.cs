using System.ComponentModel.DataAnnotations;

namespace BookReview.WebApi.Dtos
{
    public class LogoutDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
