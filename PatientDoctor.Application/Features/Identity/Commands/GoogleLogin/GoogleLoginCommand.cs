 
namespace PatientDoctor.Application.Features.Identity.Commands.GoogleLogin;
public record GoogleLoginCommand :IRequest<IResponse>
{
    public string IdToken { get; set; }
}

