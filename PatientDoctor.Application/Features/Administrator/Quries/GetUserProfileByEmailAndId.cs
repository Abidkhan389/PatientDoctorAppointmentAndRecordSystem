using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Administrator.Quries;
public   class GetUserProfileByEmailAndId :IRequest<IResponse>
{
    public string EmailOrPhoneNumber { get; set; }
    public string UserId { get; set; }
}

