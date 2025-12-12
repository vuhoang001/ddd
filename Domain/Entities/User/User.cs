using System.Text.Json.Serialization;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.ValueObject;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Domain.Entities.User;

public class User : AggregateRoot, IAuditableEntity
{
    public int Id { get; private set; }

    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public Email Email { get; private set; } = null!;

    public int? Age { get; private set; }
    public string? PhoneNumber { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public bool IsActive { get; private set; }

    [JsonIgnore] public UserSession? UserSession { get; private set; }

    public User()
    {
    }

    public User(string firstName, string lastName, int? age, string? phoneNumber, Password password,
        bool isActive, Email email)
    {
        ValidateAge(age);
        FirstName   = firstName.Trim();
        LastName    = lastName.Trim();
        Age         = age;
        PhoneNumber = phoneNumber?.Trim();
        Password    = password;
        IsActive    = isActive;
        Email       = email;
    }

    private void ValidateAge(int? age)
    {
        if (!age.HasValue) return;
        if (age < 0) throw new ValidationException("Tuổi không được nhỏ hơn 0");
        if (age < 15) throw new ValidationException("Người dùng phải từ 15 tuổi trở lên");
    }
}