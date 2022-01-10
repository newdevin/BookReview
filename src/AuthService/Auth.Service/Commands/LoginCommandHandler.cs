using Auth.Service.Repositories;
using MediatR;

namespace Auth.Service.Commands
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserToken>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptor _encryptor;
        private readonly IEmailValidator _emailValidator;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public const string Email_Or_Password_Not_Correct = "The email and password supplied is not correct";
        public const string Password_Is_Missing = "Password is missing";
        public const string Email_Is_Invalid = "Email is invalid";

        public LoginCommandHandler(IUserRepository userRepository, IEncryptor encryptor, IEmailValidator emailValidator, 
            ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _encryptor = encryptor;
            _emailValidator = emailValidator;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<UserToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var errors = Validate(request);
            if (errors.Any())
                return new Result<UserToken>(default, default, errors);

            var user = await _userRepository.Get(request.Email);
            if(user == null)
            {
                errors.Add(Email_Or_Password_Not_Correct);
                return new Result<UserToken>(default, default, errors);
            }

            var expectedHash = _encryptor.ComputeHash(request.Password, user.PasswordSalt);
            if(expectedHash != user.PasswordHash)
            {
                errors.Add(Email_Or_Password_Not_Correct);
                return new Result<UserToken>(default, default, errors);
            }
            
            var userInfo = new UserInfo(user.Email, user.FirstName, user.LastName);
            var token = _tokenService.GenerateToken(userInfo);

            var refreshToken = Guid.NewGuid().ToString();
            await _refreshTokenRepository.Insert(user.Email, refreshToken);

            return new Result<UserToken>(new UserToken { Token = token, RefreshToken = refreshToken, Email = user.Email }, "", Enumerable.Empty<string>());
        }

        private List<string> Validate(LoginCommand command)
        {
            var errors = new List<string>();
            
            if (!_emailValidator.Validate(command.Email))
                errors.Add(Email_Is_Invalid);

            if (string.IsNullOrEmpty(command.Password))
                errors.Add(Password_Is_Missing);

            return errors;
        }
    }

}
