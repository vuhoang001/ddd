using Domain.Abstractions;
using Domain.ValueObject;

namespace Domain.Entities.User;

public class UserSession : AggregateRoot, IAuditableEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }

    public Token Token { get; private set; } = null!;

    public long TokenExpiresAt { get; private set; }

    public string? DevideId { get; private set; }

    public string? IpAddress { get; private set; }

    public User? User { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public UserSession()
    {
    }

    public UserSession(int userId, Token token, long tokenExpiresAt, string? devideId, string? ipAddress)
    {
        UserId         = userId;
        Token          = token;
        TokenExpiresAt = tokenExpiresAt;
        DevideId       = devideId;
        IpAddress      = ipAddress;
    }
}