
namespace PatientDoctor.domain.Entities;
public class UserRefreshToken
{
    public int Id { get; set; }
    public string UserId { get; set; }

    public string TokenHash { get; set; }   // 🔐 hashed
    public DateTime Expires { get; set; }

    public bool IsUsed { get; set; }
    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

