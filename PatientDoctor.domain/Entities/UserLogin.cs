
namespace PatientDoctor.domain.Entities;
public class UserLogin
{
    public Guid Id { get; set; }

    public AuthProvider Provider { get; set; }

    public string ProviderKey { get; set; } = null!;

    // MUST be string because IdentityUser.Id is string
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
}

