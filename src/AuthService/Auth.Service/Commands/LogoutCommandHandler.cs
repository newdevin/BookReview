using Auth.Service.Repositories;
using MediatR;

namespace Auth.Service.Commands
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private const string User_Invalid = "User is invalid";

        public LogoutCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var errorMessages = new List<string>();
            var user = await _userRepository.Get(request.Email);
            if (user == null)
            {
                errorMessages.Add(User_Invalid);
                return new Result<bool>(false, default, errorMessages);
            }

            await _refreshTokenRepository.Delete(request.Email);

            return new Result<bool>(true, default, errorMessages);
        }
    }
}
