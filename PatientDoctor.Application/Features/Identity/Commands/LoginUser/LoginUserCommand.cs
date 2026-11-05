 
namespace PatientDoctor.Application.Features.Identity.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<IResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? TwoFactorCode { get; set; }
        public string? TwoFactorRecoveryCode { get; set; }
        // "Remember this Device" - if true and 2FA passes, a signed trusted-device
        // token is returned so this device can skip 2FA next time.
        public bool RememberDevice { get; set; }
        public string? TrustedDeviceToken { get; set; }

    }
}
