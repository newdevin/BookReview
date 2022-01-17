using BookReview.Domain;
using MediatR;

namespace BookReview.Service.Commands;

public record ChangePasswordCommand(string Email, string OriginalPassword, string Password, string RepeatPassword) : IRequest<Result<UserInfo>>;
