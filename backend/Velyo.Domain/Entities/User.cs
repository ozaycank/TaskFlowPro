using Velyo.Domain.Common.Models;

namespace Velyo.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;

    // NEW FIELDS FOR AUTHENTICATION
    public string PasswordHash { get; private set; } = null!;
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    protected User() { }

    private User(string email, string firstName, string lastName, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PasswordHash = passwordHash;
    }

    public static User Create(string email, string firstName, string lastName, string passwordHash)
    {
        return new User(email, firstName, lastName, passwordHash);
    }

    public void UpdateProfile(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void UpdateRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }
}