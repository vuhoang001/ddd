using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public (string, long) GenerateAccessToken(object userId, Action<ClaimsIdentity>? action)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()!)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject            = new ClaimsIdentity(claims),
            Expires            = expiresAt,
            Issuer             = _jwtOptions.Issuer,
            Audience           = _jwtOptions.Audience,
            SigningCredentials = creds
        };

        action?.Invoke(tokenDescriptor.Subject);

        var handler = new JsonWebTokenHandler();
        var token   = handler.CreateToken(tokenDescriptor);


        return (token, ToUnixTimeSeconds(expiresAt));
    }


    public (string, long) GenerateRefreshToken(object userId)
    {
        var expiresAt    = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
        var randomNumber = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return (Convert.ToBase64String(randomNumber), ToUnixTimeSeconds(expiresAt));
        }
    }

    public bool IsRefreshTokenValid(string? token, long refreshTokenExpriresAtUnix, bool isRevooked = false)
    {
        if (token is null) return false;

        if (isRevooked) return false;

        var currentTime = ToUnixTimeSeconds(DateTime.UtcNow);
        if (currentTime > refreshTokenExpriresAtUnix) return false;
        return true;
    }

    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key          = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = _jwtOptions.Issuer,
                ValidAudience            = _jwtOptions.Audience,
                IssuerSigningKey         = new SymmetricSecurityKey(key),
                ClockSkew                = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private long ToUnixTimeSeconds(DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }
}