
namespace PatientDoctor.Infrastructure.Repositories.GoogleLogin;
public class GoogleLoginRepository : IGoogleTokenValidator
{
    private readonly IConfiguration _config;

    public GoogleLoginRepository(IConfiguration config)
    {
        _config = config;
    }
    public async Task<GoogleJsonWebSignature.Payload?> ValidateAsync(string idToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { _config["GoogleAuth:ClientId"] }
        };
        return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
    }
}

