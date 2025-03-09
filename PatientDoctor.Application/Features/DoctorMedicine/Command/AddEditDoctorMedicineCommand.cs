

using MediatR;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.DoctorMedicine.Command;
public class AddEditDoctorMedicineCommand : TableParam, IRequest<IResponse>
{
    public Guid? Id { get; set; }

    public Guid MedicineId { get; set; }
    public List<DoctorIds> DoctorIds { get; set; }

    public Guid? UserId { get; set; }
}
public class DoctorIds
{
    public Guid DoctorId { get; set; }
}
