using System.Text;
using System.Text.Json;
using auth_service.Models;

namespace auth_service.Services
{
    public class AiAnomalyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AiAnomalyService> _logger;

        public AiAnomalyService(HttpClient httpClient, ILogger<AiAnomalyService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:8000"); // AI Service port
            _logger = logger;
        }

        public async Task<AiAnomalyResponse> AnalyzeLoginAsync(LoginEvent loginEvent)
        {
            try
            {
                var features = new
                {
                    hour = loginEvent.LoginTime.Hour,
                    failedAttempts = loginEvent.FailedAttempts,
                    newIp = 1,                    // Mund ta bësh dinamik më vonë
                    successRate = loginEvent.Success ? 1.0 : 0.0,
                    loginFrequency = 3,           // Mund ta bësh dinamik
                    countryChanged = 0,
                    impossibleTravel = 0,
                    distanceKm = 0.0,
                    hoursSinceLastLogin = 24.0,
                    userAgentChanged = 0
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(features), 
                    Encoding.UTF8, 
                    "application/json");

                var response = await _httpClient.PostAsync("/score", content);
                
                if (!response.IsSuccessStatusCode)
                    return new AiAnomalyResponse { Anomaly = false, RiskScore = 0 };

                var result = await response.Content.ReadFromJsonAsync<AiAnomalyResponse>();
                return result ?? new AiAnomalyResponse { Anomaly = false, RiskScore = 0 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling AI Service");
                return new AiAnomalyResponse { Anomaly = false, RiskScore = 0 };
            }
        }
    }

    public class AiAnomalyResponse
    {
        public bool Anomaly { get; set; }
        public double RiskScore { get; set; }
        public string RiskLevel { get; set; } = "NORMAL";
        public bool BlockAccount { get; set; }
    }
}
