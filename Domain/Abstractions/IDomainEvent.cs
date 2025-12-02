namespace Domain.Abstractions;

public interface IDomainEvent
{
    public DateTime CreatedAt { get; }

    public string? Type => GetType().AssemblyQualifiedName;
}