namespace Application.Common;

public class TokenResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public long AccessTokenExpiration { get; set; }
    public long RefreshTokenExpiration { get; set; }
}