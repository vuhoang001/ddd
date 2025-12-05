namespace Domain.Abstractions;

public interface IDomainEvent
{
    public DateTime CreatedAt { get; }

    public string? Type => GetType().AssemblyQualifiedName;
}

public interface IDeferredDomainEvent : IDomainEvent
{
}

public interface IImmediateDomainEvent : IDomainEvent
{
}