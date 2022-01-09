using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Commands
{
    public record RefreshTokenCommand(string Email, string RefreshToken) : IRequest<Result<UserToken>>;


}
