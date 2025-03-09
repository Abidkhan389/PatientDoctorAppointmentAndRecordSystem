

using PatientDoctor.Application.Features.DoctorMedicine.Command;

namespace PatientDoctor.Application.Features.DoctorMedicine.Query;
public class VM_DoctorMedicine
{
    public Guid MedicineId { get; set; }
    public List<DoctorIds> DoctorIds { get; set; }
}

