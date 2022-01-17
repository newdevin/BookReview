using BookReview.Domain;

namespace BookReview.Service
{
    public interface ITokenService
    {
        string GenerateToken(UserInfo user);
    }


}
