using Application.Common;
using Application.Features.User.Dtos;
using MediatR;

namespace Application.Features.User.Commands;

public record SignUpCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Result<SignUpResponse>>
{
}