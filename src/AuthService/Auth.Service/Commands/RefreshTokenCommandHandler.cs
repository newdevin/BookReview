using Auth.Service.Repositories;
using MediatR;

namespace Auth.Service.Commands
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<UserToken>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;

        private const string Invalid_Refresh_Token = "Invalid refresh token";

        public RefreshTokenCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<UserToken>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            var user = await _userRepository.Get(request.Email);
            if (user == null)
            {
                errors.Add(Invalid_Refresh_Token);
                return new Result<UserToken>(default, default, errors);
            }

            var storedRefreshToken = await _refreshTokenRepository.GetRefreshToken(request.Email);
            if (storedRefreshToken == null || storedRefreshToken != request.RefreshToken)
            {
                errors.Add(Invalid_Refresh_Token);
                return new Result<UserToken>(default, default, errors);
            }

            var userInfo = new UserInfo(user.Email, user.FirstName, user.LastName);
            var token = _tokenService.GenerateToken(userInfo);

            var refreshToken = Guid.NewGuid().ToString();
            await _refreshTokenRepository.Insert(user.Email, refreshToken);

            return new Result<UserToken>(new UserToken { Token = token, RefreshToken = refreshToken, Email = user.Email }, "", Enumerable.Empty<string>());
        }
    }


}
