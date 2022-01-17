using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookReview.Service.Commands
{
    public record LogoutCommand(string Email) : IRequest<Result<bool>>;
}
