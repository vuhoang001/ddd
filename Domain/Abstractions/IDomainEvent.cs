using MediatR;

namespace Domain.Abstractions;

public interface IDomainEvent : INotification
{
    public string Type => GetType().Name;
}

public interface IImmediateDomainEvent : IDomainEvent
{
}

public interface IDelayedDomainEvent : IDomainEvent
{
}