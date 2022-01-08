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
        private readonly IPasswordValidator _passwordValidator;
        
        public const string Email_Is_Invalid = "Email is invalid";

        public const string User_Exists = "Email already exists";
        public const string Success = "User created";


        private readonly int _minimumPasswordLength = 8;

        public RegisterCommandHandler(IAuthConfiguration authConfiguration, IUserRepository userRepository, IEncryptor encryptor, IPasswordValidator passwordValidator)
        {
            _authConfiguration = authConfiguration;
            _userRepository = userRepository;
            _encryptor = encryptor;
            _passwordValidator = passwordValidator;
            _minimumPasswordLength = _authConfiguration.GetMinimumPasswordLength() ?? 8;
        }

        public async Task<Result<User>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var errorMessages = _passwordValidator.Validate(request.Password, request.RepeatPassword, _minimumPasswordLength).ToList();
            
            var emailValidationMessage = ValidateEmail(request.Email);

            if (emailValidationMessage != null)
                errorMessages.Add(emailValidationMessage);

            if (errorMessages.Any())
                return new Result<User>(default, default, errorMessages);

            User user = await _userRepository.Get(request.Email);
            if (user != null)
            {
                errorMessages.Add(User_Exists);
                return new Result<User>(default, default, errorMessages);
            }

            var salt = _encryptor.GenerateSalt();
            var hash = _encryptor.ComputeHash(request.Password, salt);

            user = await _userRepository.InsertUser(new User(request.Email, salt, hash, DateTime.UtcNow, DateTime.UtcNow));

            return new Result<User>(user, Success, errorMessages);
        }

        private string? ValidateEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
                return Email_Is_Invalid;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                return Email_Is_Invalid;
            }
            return null;
        }

       
    }
}
