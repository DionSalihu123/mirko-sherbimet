namespace auth_service.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsBlocked { get; set; } = false;

    public DateTime? BlockedUntil { get; set; }
}
