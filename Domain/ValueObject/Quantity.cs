using Domain.Exceptions;

namespace Domain.ValueObject;

public class Quantity : IEquatable<Quantity>
{
    public int Value { get; }

    public Quantity(int value)
    {
        Value = value;
    }


    public bool Equals(Quantity? other)
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
        return Equals((Quantity)obj);
    }

    public override int GetHashCode()
    {
        return Value;
    }

    public override string ToString() => Value.ToString();

    public static Quantity operator +(Quantity a, Quantity b) => new(a.Value + b.Value);

    public static Quantity operator -(Quantity a, Quantity b)
    {
        if (a.Value < b.Value)
            throw new DomainException("Số lượng không được âm");

        return new Quantity(a.Value - b.Value);
    }

    public static bool operator >(Quantity a, Quantity b) => a.Value > b.Value;
    public static bool operator <(Quantity a, Quantity b) => a.Value < b.Value;
}