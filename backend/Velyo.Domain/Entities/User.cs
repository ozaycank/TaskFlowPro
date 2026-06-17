using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;

    protected User() { }

    private User(string email, string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public static User Create(string email, string firstName, string lastName)
    {
        return new User(email, firstName, lastName);
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}