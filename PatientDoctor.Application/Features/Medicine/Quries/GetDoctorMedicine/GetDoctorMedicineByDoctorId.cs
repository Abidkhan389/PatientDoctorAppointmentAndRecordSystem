
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetDoctorMedicine;
 public   class GetDoctorMedicineByDoctorId : IRequest<IResponse>
{
    public string DoctorId { get; set; }
}

