using System.Security.Claims;
using Application.Common;

namespace Application.Interfaces;

public interface IJwtService
{
    (string, long )         GenerateAccessToken(object userId, Action<ClaimsIdentity>? action);
    (string, long)          GenerateRefreshToken(object userId);
    public bool             IsRefreshTokenValid(string token, long expriresAt, bool isRevooked = false);
    public ClaimsPrincipal? ValidateAccessToken(string token);
}