namespace Auth.Service
{
    public interface IEmailValidator
    {
        bool Validate(string email);
    }
}