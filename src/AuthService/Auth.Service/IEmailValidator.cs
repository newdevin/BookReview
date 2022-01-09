namespace Auth.Service
{
    public interface IEmailValidator
    {
        bool Validate(string email);
    }

    public class EmailValidator : IEmailValidator
    {
        public const string Email_Is_Invalid = "Email is invalid";
        public bool Validate(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}