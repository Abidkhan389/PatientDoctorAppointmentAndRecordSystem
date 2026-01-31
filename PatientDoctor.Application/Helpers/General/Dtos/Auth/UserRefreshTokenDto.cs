
namespace PatientDoctor.Application.Helpers.General.Dtos.Auth;
public class UserRefreshTokenDto
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public bool IsUsed { get; set; }
    public string UserId { get; set; }
}

