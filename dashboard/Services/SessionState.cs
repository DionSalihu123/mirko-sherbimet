namespace dashboard.Services;

public class SessionState
{
    public string? Email { get; set; }
    public string? JwtToken { get; set; }
    public string? LastMessage { get; set; }
    public bool IsError { get; set; }

    public void SetLogin(string email, string token, string message)
    {
        Email = email;
        JwtToken = token;
        LastMessage = message;
        IsError = false;
    }

    public void SetError(string message)
    {
        LastMessage = message;
        IsError = true;
    }

    public void Clear()
    {
        Email = null;
        JwtToken = null;
        LastMessage = null;
        IsError = false;
    }
}
