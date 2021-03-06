using BookReview.Domain;
using BookReview.Service.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.Service.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegistrationInfo>>
    {
        private readonly IAuthConfiguration _authConfiguration;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptor _encryptor;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IEmailValidator _emailValidator;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        public const string Email_Is_Invalid = "Email is invalid";
        public const string User_Exists = "Email already exists";
        public const string Success = "User created";

        private readonly int _minimumPasswordLength = 8;

        public RegisterCommandHandler(IAuthConfiguration authConfiguration, IUserRepository userRepository,
            IEncryptor encryptor, IPasswordValidator passwordValidator, IEmailValidator emailValidator, IEmailVerificationRepository emailVerificationRepository)
        {
            _authConfiguration = authConfiguration;
            _userRepository = userRepository;
            _encryptor = encryptor;
            _passwordValidator = passwordValidator;
            _emailValidator = emailValidator;
            _emailVerificationRepository = emailVerificationRepository;
            _minimumPasswordLength = _authConfiguration.GetMinimumPasswordLength() ?? 8;
        }

        public async Task<Result<RegistrationInfo>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var errorMessages = Validate(request);
            if (errorMessages.Any())
                return new Result<RegistrationInfo>(default, default, errorMessages);

            User user = await _userRepository.Get(request.Email);
            if (user != null)
            {
                errorMessages.Add(User_Exists);
                return new Result<RegistrationInfo>(default, default, errorMessages);
            }

            var salt = _encryptor.GenerateSalt();
            var hash = _encryptor.ComputeHash(request.Password, salt);

            user = await _userRepository.Insert(new User(request.Email, request.FirstName, request.LastName, false, salt, hash, DateTime.UtcNow, DateTime.UtcNow));

            var code = Guid.NewGuid().ToString();
            await _emailVerificationRepository.AddCode(user.Email, code);

            return new Result<RegistrationInfo>(new RegistrationInfo(user.Email, user.FirstName, user.LastName, code), Success, errorMessages);
        }
        
        public List<string> Validate(RegisterCommand request)
        {
            var errorMessages = _passwordValidator.Validate(request.Password, request.RepeatPassword, _minimumPasswordLength).ToList();

            var emailValidationMessage = _emailValidator.Validate(request.Email) ? null : Email_Is_Invalid;

            if (emailValidationMessage != null)
                errorMessages.Add(emailValidationMessage);

            return errorMessages;
        }

    }
}
