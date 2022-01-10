using Auth.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Commands;

public record RegisterCommand(string Email, string FirstName,string LastName, string Password, string RepeatPassword) : IRequest<Result<RegistrationInfo>>;
