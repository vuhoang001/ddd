using Application.Common;
using Application.Interfaces;
using Domain.Repositories;
using Domain.ValueObject;
using MediatR;

namespace Application.Features.User.Commands;

public class SignUpCommandHandle(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<SignUpCommand, Result>
{
    public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = Password.Create(request.Password);
        var newUser = new Domain.Entities.User.User(
            firstName: request.FirstName,
            lastName: request.LastName,
            age: null,
            phoneNumber: null,
            passwordHash,
            true,
            request.Email
        );


        new Domain.Entities.User.User(
            )

        userRepository.Add(newUser);
        await unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}