namespace Domain.Abstractions;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}

public static class AuditableEntityExtensions
{
    public static void SetCreatedAt(this IAuditableEntity auditableEntity, DateTime now)
    {
        var property = auditableEntity.GetType().GetProperty(nameof(IAuditableEntity.CreatedAt));
        property!.SetValue(auditableEntity, now);
    }

    public static void SetUpdateAt(this IAuditableEntity auditableEntity, DateTime now)
    {
        var property = auditableEntity.GetType().GetProperty(nameof(IAuditableEntity.UpdatedAt));
        property!.SetValue(auditableEntity, now);
    }
}