using Application.Common;
using MediatR;

namespace Application.Features.User.Commands;

public record SignUpCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result>
{
}