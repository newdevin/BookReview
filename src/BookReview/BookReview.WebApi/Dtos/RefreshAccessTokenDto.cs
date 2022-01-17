using System.ComponentModel.DataAnnotations;

namespace BookReview.WebApi.Dtos
{
    public class RefreshAccessTokenDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
