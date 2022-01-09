namespace Auth.Service
{
    public class PasswordValidator : IPasswordValidator
    {
        public const string Password_Not_Long_Enough = "The password length should be atlease {0} character long";
        public const string Password_Is_Missing_Capital_Letter = "Password do not have any capital letter";
        public const string Password_Is_Missing_Small_Letter = "Password do not have any small letter";
        public const string Password_Is_Missing_Number = "Password do not have any number";
        public const string Password_Is_Empty = "The password is empty or contains only spaces";
        public const string RepeatPassword_Does_Not_Match = "The repeat password do not match with password";

        public IEnumerable<string> Validate(string password, string repeatPassword, int minPasswordLength)
        {
            var errors = Validate(password, minPasswordLength).ToList();
            if (password != repeatPassword)
                errors.Add(RepeatPassword_Does_Not_Match);

            return errors;
        }

        private IEnumerable<string> Validate(string password, int minPasswordLength)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add(Password_Is_Empty);
                return errors;
            }
            if (password.Length < minPasswordLength)
                errors.Add(string.Format(Password_Not_Long_Enough, minPasswordLength));
            if (password.All(ch => char.IsUpper(ch) == false))
                errors.Add(Password_Is_Missing_Capital_Letter);
            if (password.All(ch => char.IsLower(ch) == false))
                errors.Add(Password_Is_Missing_Small_Letter);
            if (password.All(ch => char.IsDigit(ch) == false))
                errors.Add(Password_Is_Missing_Number);

            return errors;
        }

    }
}
