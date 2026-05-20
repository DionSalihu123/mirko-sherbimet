namespace auth_service.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    // -------------------------
    // AI / Security fields
    // -------------------------

    public int FailedLoginAttempts { get; set; } = 0;

    public DateTime? BlockedUntil { get; set; }

    public string? LastLoginIp { get; set; }

    public string? LastLoginCountry { get; set; }

    public double? LastLoginLatitude { get; set; }

    public double? LastLoginLongitude { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public int LoginCount { get; set; } = 0;
}
