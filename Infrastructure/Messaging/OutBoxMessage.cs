using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Messaging;

public class OutBoxMessage
{
    public int Id { get; set; }
    [MaxLength(255)] public string Type { get; set; } = null!;
    [MaxLength(255)] public string Payload { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    [MaxLength(2000)] public string? Error { get; set; }

    public int Attempt { get; set; }

    [MaxLength(255)] public string? LockId { get; set; }

    public DateTime? LockedUntil { get; set; }
}