namespace auth_service.Models;

public class LoginEvent
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;
    public bool Success { get; set; }

    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public bool IsAnomaly { get; set; }
    public float? RiskScore { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
