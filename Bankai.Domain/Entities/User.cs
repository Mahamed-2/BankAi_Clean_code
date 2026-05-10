using Bankai.Domain.Enums;

namespace Bankai.Domain.Entities;

/// <summary>
/// Application user — stored in the Domain layer.
/// Authentication details managed by Infrastructure.
/// </summary>
public class User
{
    public int Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
    private readonly List<Order> _orders = new();

    public User(string email, string passwordHash, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    protected User() { }

    public void PromoteToAdmin() => Role = UserRole.Admin;
    public void DemoteToUser() => Role = UserRole.User;
}
