using Domain.Exceptions;

namespace Domain.ValueObject;

public class Price : IEquatable<Price>
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value < 0) throw new DomainException("Price cannot be negative");
        Value = value;
    }

    public bool Equals(Price? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals((Price)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString() => Value.ToString("N0") + " VND";

    public static Price operator +(Price a, Price b) => new(a.Value + b.Value);

    public static Price operator -(Price a, Price b)
    {
        return new Price(a.Value - b.Value);
    }

    public static bool operator >(Price a, Price b) => a.Value > b.Value;
    public static bool operator <(Price a, Price b) => a.Value < b.Value;
}