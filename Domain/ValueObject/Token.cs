using System.Security.Cryptography;
using System.Text;

namespace Domain.ValueObject;

public class Token : IEquatable<Token>
{
    public string Value { get; }
    public string Hash { get; }

    private Token(string value, string hash)
    {
        Value = value;
        Hash  = hash;
    }


    public static Token Create(string value)
    {
        var hash = ComputeHash(value);
        return new Token(value, hash);
    }

    public bool Equals(Token? other)
    {
        if (other is null) return false;
        return Hash == other.Hash;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Token)obj);
    }

    public override int GetHashCode()
    {
        return Hash.GetHashCode();
    }


    private static string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var       bytes  = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}