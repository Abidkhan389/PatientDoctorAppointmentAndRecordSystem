using MediatR;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetAllByProc
{
	public class GetDoctorCheckUpFeeDetailsList : TableParam, IRequest<IResponse>
    {
        public string? DoctorName { get; set; }
        public int? DoctorFee { get; set; }
    }
}

