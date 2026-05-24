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
            _httpClient.BaseAddress = new Uri("http://localhost:8000"); // Porti i AI Service (FastAPI)
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
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
                    newIp = loginEvent.IsNewIp ? 1 : 0,
                    successRate = loginEvent.Success ? 1.0 : 0.0,
                    loginFrequency = loginEvent.LoginFrequency,
                    countryChanged = loginEvent.CountryChanged ? 1 : 0,
                    impossibleTravel = loginEvent.ImpossibleTravel ? 1 : 0,
                    distanceKm = loginEvent.DistanceKm,
                    hoursSinceLastLogin = loginEvent.HoursSinceLastLogin,
                    userAgentChanged = loginEvent.UserAgentChanged ? 1 : 0
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(features),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/score", content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"AI Service returned {response.StatusCode}");
                    return new AiAnomalyResponse { Anomaly = false, RiskScore = 0, BlockAccount = false };
                }

                var result = await response.Content.ReadFromJsonAsync<AiAnomalyResponse>();
                return result ?? new AiAnomalyResponse { Anomaly = false, RiskScore = 0, BlockAccount = false };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling AI Anomaly Service");
                return new AiAnomalyResponse { Anomaly = false, RiskScore = 0, BlockAccount = false };
            }
        }
    }

    // Response model nga AI Service
    public class AiAnomalyResponse
    {
        public bool Anomaly { get; set; }
        public double RiskScore { get; set; }
        public string RiskLevel { get; set; } = "NORMAL";
        public bool BlockAccount { get; set; }
    }
}
