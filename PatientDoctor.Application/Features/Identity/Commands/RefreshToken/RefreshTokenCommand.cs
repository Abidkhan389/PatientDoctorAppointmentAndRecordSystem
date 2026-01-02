
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Identity.Commands.RefreshToken;
public class RefreshTokenCommand : IRequest<IResponse>
{
    public string RefreshToken { get; set; }
}


