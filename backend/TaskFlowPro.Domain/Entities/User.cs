using TaskFlowPro.Domain.Common.Models;

namespace TaskFlowPro.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    // EF Core requires a parameterless constructor
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
        // Business logic validations can be added here
        return new User(email, firstName, lastName);
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}