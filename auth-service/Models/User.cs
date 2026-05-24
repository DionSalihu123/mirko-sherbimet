using System.ComponentModel.DataAnnotations;

namespace auth_service.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Fusha shtesë për AI / Blocking (mund t'i shtosh më vonë)
        public bool IsBlocked { get; set; } = false;
        public DateTime? BlockedUntil { get; set; }
    }
}
