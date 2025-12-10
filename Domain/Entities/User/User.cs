using Domain.Abstractions;
using Domain.ValueObject;

namespace Domain.Entities.User;

public class User : AggregateRoot, IAuditableEntity
{
    public int Id { get; private set; }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public string Email { get; private set; } = null!;

    public int? Age { get; private set; }
    public string? PhoneNumber { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public bool IsActive { get; private set; }

    public User()
    {
    }

    public User(string firstName, string lastName, int? age, string? phoneNumber, Password password,
        bool isActive, string email)
    {
        FirstName   = firstName;
        LastName    = lastName;
        Age         = age;
        PhoneNumber = phoneNumber;
        Password    = password;
        IsActive    = isActive;
        Email       = email;
    }
}