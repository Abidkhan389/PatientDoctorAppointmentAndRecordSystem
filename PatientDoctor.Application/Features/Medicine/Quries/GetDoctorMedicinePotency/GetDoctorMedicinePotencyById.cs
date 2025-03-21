
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetDoctorMedicinePotency;
public   class GetDoctorMedicinePotencyById : IRequest<IResponse>
{
    public Guid MedicineId { get; set; }
}

