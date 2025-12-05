using System.Text.Json;
using Application.Interfaces;
using Domain.Abstractions;

namespace Infrastructure.Messaging;

public sealed class SystemTextJsonEventSerializer : IEventSerializer
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
    };

    public string Serialize(IDomainEvent e) => JsonSerializer.Serialize(e, e.GetType(), JsonOptions);

    public IDomainEvent Deserialize(string type, string payload)
    {
        var t = Type.GetType(type, throwOnError: true)!;
        return (IDomainEvent)JsonSerializer.Deserialize(payload, t, JsonOptions)!;
    }
}