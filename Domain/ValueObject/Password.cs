namespace Domain.ValueObject;

public class Password : IEquatable<Password>
{
    public string Hash { get; private set; } = null!;
    public string Salt { get; private set; } = null!;

    public Password()
    {
    }

    public Password(string hash, string salt)
    {
        Hash = hash;
        Salt = salt;
    }

    public static Password Create(string password)
    {
        var slat = Guid.NewGuid().ToString("N");
        var hash = ComputeHash(password, slat);
        return new Password(hash, slat);
    }

    public bool Verify(string password)
    {
        var hash = ComputeHash(password, Salt);
        return hash == Hash;
    }

    private static string ComputeHash(string password, string salt)
    {
        using var sha256    = System.Security.Cryptography.SHA256.Create();
        var       combined  = password + salt;
        var       bytes     = System.Text.Encoding.UTF8.GetBytes(combined);
        var       hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    public bool Equals(Password? other)
    {
        if (other is null) return false;
        return Hash == other.Hash && Salt == other.Salt;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Password)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hash, Salt);
    }
}