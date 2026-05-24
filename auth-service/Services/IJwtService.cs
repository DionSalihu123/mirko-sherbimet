using auth_service.Models;

namespace auth_service.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
