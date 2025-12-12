using Application.Common;
using Application.Features.User.Dtos;
using Application.Interfaces;
using Domain.Entities.User;
using Domain.Repositories;
using Domain.ValueObject;
using MediatR;

namespace Application.Features.User.Commands;

public class SignUpCommandHandle(
    IUserRepository userRepository,
    IUserSessionRepository userSessionRepository,
    IUnitOfWork unitOfWork,
    IJwtService jwtService)
    : IRequestHandler<SignUpCommand, Result<SignUpResponse>>
{
    public async Task<Result<SignUpResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            #region Kiểm tra xem có trùng Email nào hay không?

            var existingUser = await userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            if (existingUser is not null) return Result<SignUpResponse>.BadRequest("Email đã tồn tại");

            #endregion

            var passwordHash = Password.Create(request.Password);
            var email        = Email.Create(request.Email);
            var newUser = new Domain.Entities.User.User(
                firstName: request.FirstName,
                lastName: request.LastName,
                age: null,
                phoneNumber: null,
                passwordHash,
                true,
                email
            );

            userRepository.Add(newUser);
            await unitOfWork.SaveChangesAsync();

            var (accessToken, accessExp) =
                jwtService.GenerateAccessToken(newUser.Id, null);

            var (refreshToken, refreshExp) =
                jwtService.GenerateRefreshToken(newUser.Id);

            var refreshTokenHash = Token.Create(refreshToken);


            userSessionRepository.Add(
                new UserSession(
                    userId: newUser.Id,
                    token: refreshTokenHash,
                    tokenExpiresAt: refreshExp,
                    devideId: null,
                    ipAddress: null
                )
            );
            await unitOfWork.SaveChangesAsync();

            return Result<SignUpResponse>.Success(new SignUpResponse
            {
                AccessToken            = accessToken,
                AccessTokenExpiration  = accessExp,
                RefreshToken           = refreshToken,
                RefreshTokenExpiration = refreshExp,
                User                   = newUser
            });
        });
    }
}