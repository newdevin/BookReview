using Auth.Domain;
using Auth.Service.Repositories;
using MediatR;

namespace Auth.Service.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<UserInfo>>
{
    private readonly IAuthConfiguration _authConfiguration;
    private readonly IUserRepository _userRepository;
    private readonly IEncryptor _encryptor;
    private readonly IPasswordValidator _passwordValidator;
    private readonly IEmailValidator _emailValidator;

    private readonly int _minimumPasswordLength = 8;
    private const string Email_Is_Invalid = "Email is invalid";
    public const string User_Not_Found = "User not found";
    public const string User_Password_Not_Matched = "User password does not match";
    public const string Password_Updated = "Password updated";

    public ChangePasswordCommandHandler(IAuthConfiguration authConfiguration, IUserRepository userRepository, IEncryptor encryptor, IPasswordValidator passwordValidator, IEmailValidator emailValidator)
    {
        _authConfiguration = authConfiguration;
        _userRepository = userRepository;
        _encryptor = encryptor;
        _passwordValidator = passwordValidator;
        _emailValidator = emailValidator;
        _minimumPasswordLength = _authConfiguration.GetMinimumPasswordLength() ?? 8;

    }
    public async Task<Result<UserInfo>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var errorMessages = Validate(request);
        if (errorMessages.Any())
            return new Result<UserInfo>(default, default, errorMessages);

        var user = await _userRepository.Get(request.Email);
        if (user == null)
        {
            errorMessages.Add(User_Not_Found);
            return new Result<UserInfo>(default, default, errorMessages);
        }

        var expectedHash = _encryptor.ComputeHash(request.OriginalPassword, user.PasswordSalt);
        if (expectedHash != user.PasswordHash)
        {
            errorMessages.Add(User_Password_Not_Matched);
            return new Result<UserInfo>(default, default, errorMessages);
        }

        await UpdateUser(request, user);

        return new Result<UserInfo>(new UserInfo(request.Email, user.FirstName, user.LastName), Password_Updated, Enumerable.Empty<string>());
    }

    private async Task UpdateUser(ChangePasswordCommand request, User user)
    {
        var newSalt = _encryptor.GenerateSalt();
        var newHash = _encryptor.ComputeHash(request.Password, newSalt);
        var newUser = user.UpdatePassword(newSalt, newHash);
        await _userRepository.Update(newUser);
    }

    private List<string> Validate(ChangePasswordCommand request)
    {
        var errorMessages = _passwordValidator.Validate(request.Password, request.RepeatPassword, _minimumPasswordLength).ToList();

        var emailValidationMessage = _emailValidator.Validate(request.Email) ? null : Email_Is_Invalid;

        if (emailValidationMessage != null)
            errorMessages.Add(emailValidationMessage);

        return errorMessages;
    }
}