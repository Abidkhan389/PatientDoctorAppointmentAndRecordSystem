
namespace PatientDoctor.Application.Contracts.Persistance.GoogeLogin;
public interface IGoogleTokenValidator
{   
    Task<GoogleJsonWebSignature.Payload?> ValidateAsync(string idToken);
}

