namespace auth_service.Models;

using System.ComponentModel.DataAnnotations;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    // -------------------------
    // AI / Fileds per siguri  
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
