

using MediatR;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.DoctorMedicine.Command;
public class AddEditDoctorMedicineCommand : IRequest<IResponse>
{
    public Guid? Id { get; set; }

    public Guid MedicineId { get; set; }
    public List<DoctorIds> DoctorIds { get; set; }

    public string? UserId { get; set; }
}
public class DoctorIds
{
    public string DoctorId { get; set; }
}
