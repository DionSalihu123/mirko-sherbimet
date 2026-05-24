namespace auth_service.Models
{
    public class LoginEvent
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool Success { get; set; }
        public int FailedAttempts { get; set; }

        public bool IsNewIp { get; set; } = true;
        public bool CountryChanged { get; set; } = false;
        public bool ImpossibleTravel { get; set; } = false;
        public double DistanceKm { get; set; } = 0.0;
        public double HoursSinceLastLogin { get; set; } = 24.0;
        public bool UserAgentChanged { get; set; } = false;
        public int LoginFrequency { get; set; } = 1;
    }
}
