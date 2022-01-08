using Auth.Domain;
using Auth.Service.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<User>>
    {
        private readonly IAuthConfiguration _authConfiguration;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptor _encryptor;
        public readonly string Password_Not_Long_Enough = "The password length should be atlease {MinPasswordLength} character long";
        public const string Password_Is_Empty = "The password is empty or contains only spaces";
        public const string RepeatPassword_Does_Not_Match = "The repeat password do not match with password";
        public const string Email_Is_Invalid = "Email is invalid";
        public const string User_Exists = "Email already exists";
        public const string Success = "User created";

        private readonly int _minimumPasswordLength = 8;

        public RegisterCommandHandler(IAuthConfiguration authConfiguration, IUserRepository userRepository, IEncryptor encryptor)
        {
            _authConfiguration = authConfiguration;
            _userRepository = userRepository;
            _encryptor = encryptor;
            _minimumPasswordLength = _authConfiguration.GetMinimumPasswordLength() ?? 8;
            Password_Not_Long_Enough = string.Format(Password_Not_Long_Enough, _minimumPasswordLength);
        }

        public async Task<Result<User>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var messages = ValidatePasswords(request.Password, request.RepeatPassword);
            var emailValidationMessage = ValidateEmail(request.Email);
            if (emailValidationMessage != null)
                messages.Add(emailValidationMessage);

            if (messages.Any())
                return new Result<User>(default, default, messages);

            User user = await _userRepository.Get(request.Email);
            if (user != null)
            {
                messages.Add(User_Exists);
                return new Result<User>(default, default, messages);
            }

            var salt = _encryptor.GenerateSalt();
            var hash = _encryptor.ComputeHash(request.Password, salt);

            user = await _userRepository.InsertUser(new User(request.Email, salt, hash, DateTime.UtcNow, DateTime.UtcNow));

            return new Result<User>(user, Success, messages);
        }

        private string ValidateEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return Email_Is_Invalid;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return string.Empty;
            }
            catch
            {
                return Email_Is_Invalid;
            }
        }

        private List<string> ValidatePasswords(string password, string repeatPassword)
        {
            var messages = new List<string>();
            if (string.IsNullOrWhiteSpace(password))
                messages.Add(Password_Is_Empty);
            if (password?.Length < _minimumPasswordLength)
                messages.Add(Password_Not_Long_Enough);
            if (password != repeatPassword)
                messages.Add(RepeatPassword_Does_Not_Match);

            return messages;

        }
    }
}
