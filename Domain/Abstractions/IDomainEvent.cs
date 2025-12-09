using MediatR;

namespace Domain.Abstractions;

public interface IDomainEvent : INotification
{
    public string Type => GetType().Name;
}