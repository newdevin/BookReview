namespace BookReview.Service
{
    public interface IEmailValidator
    {
        bool Validate(string email);
    }
}