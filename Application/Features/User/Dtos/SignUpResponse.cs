namespace Application.Features.User.Dtos;

public class SignUpResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public long AccessTokenExpiration { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public long RefreshTokenExpiration { get; set; }
    
    public Domain.Entities.User.User User { get; set; }
}