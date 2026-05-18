namespace auth_service.Models;

public class LoginEvent
{
    public int Id { get; set; }

    public string Email { get; set; } = "";
    public bool Success { get; set; }

    public string IpAddress { get; set; } = "";
    public string UserAgent { get; set; } = "";

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
