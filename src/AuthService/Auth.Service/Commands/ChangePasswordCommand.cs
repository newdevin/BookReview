using Auth.Domain;
using MediatR;

namespace Auth.Service.Commands;

public record ChangePasswordCommand(string Email, string OriginalPassword, string Password, string RepeatPassword) : IRequest<Result<UserInfo>>;
