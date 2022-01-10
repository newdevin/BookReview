using Auth.Service.Repositories;
using MediatR;

namespace Auth.Service.Commands
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailVerificationRepository _emailVerificationRepository;
        private readonly IEmailValidator _emailValidator;

        public const string Email_Invalid = "Email is invalid";
        public const string Unable_To_Verify = "Unable to verify";
        public const string User_Verified = "User verified";

        public VerifyEmailCommandHandler(IUserRepository userRepository, IEmailVerificationRepository emailVerificationRepository, IEmailValidator emailValidator)
        {
            _userRepository = userRepository;
            _emailVerificationRepository = emailVerificationRepository;
            _emailValidator = emailValidator;
        }
        public async Task<Result<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var errors = Validate(request);
            if (errors.Any())
                return new Result<bool>(false, default, errors);

            var user = await _userRepository.Get(request.Email);
            if (user == null)
            {
                errors.Add(Unable_To_Verify);
                return new Result<bool>(false, default, errors);
            }

            if (user.EmailVerified)
                return new Result<bool>(true, User_Verified, Enumerable.Empty<string>());

            var existingCode = await _emailVerificationRepository.GetCode(request.Email);
            if(existingCode == null || existingCode != request.Code)
            {
                errors.Add(Unable_To_Verify);
                return new Result<bool>(false, default, errors);
            }

            user = user.VerifyEmail(true);
            await _userRepository.Update(user);
            await _emailVerificationRepository.DeleteCode(user.Email);


            return new Result<bool>(true, User_Verified, Enumerable.Empty<string>());
        }

        private List<string> Validate(VerifyEmailCommand command)
        {
            var errors = new List<string>();

            if (!_emailValidator.Validate(command.Email))
            {
                errors.Add(Email_Invalid);
            }

            return errors;
        }
    }
}
