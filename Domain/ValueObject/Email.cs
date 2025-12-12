using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.ValueObject;

public class Email : IEquatable<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }


    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException("Trường email không được để trống");

        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ValidationException("Giá trị trường Email không hợp lệ");

        return new Email(value.ToLowerInvariant());
    }


    public bool Equals(Email? other)
    {
        if (other is null) return false;
        return other.Value == Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Email)obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}