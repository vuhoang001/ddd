using Domain.Abstractions;

namespace Application.Interfaces;

public interface IEventSerializer
{
    string       Serialize(IDomainEvent @e);
    IDomainEvent Deserialize(string type, string payload);
}

