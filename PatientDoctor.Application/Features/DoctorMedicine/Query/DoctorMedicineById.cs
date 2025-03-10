using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorMedicine.Query;
public class DoctorMedicineById : IRequest<IResponse>
{
    public DoctorMedicineById(Guid medicineId)
    {
        this.Medicineid = medicineId;
    }
    public Guid Medicineid { get; set; }
}

