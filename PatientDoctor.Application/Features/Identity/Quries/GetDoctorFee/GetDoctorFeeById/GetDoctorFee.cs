
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Identity.Quries.GetDoctorFee.GetDoctorFeeById;
public class GetDoctorFee : IRequest<IResponse>
{
    public string DoctorId { get; set; }
    public GetDoctorFee(string DoctorId)
    {
        this.DoctorId = DoctorId;
    }
}

