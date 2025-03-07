
using PatientDoctor.Application.Helpers.General;
namespace PatientDoctor.Application.Features.Patient.Quries.GetDoctorSlots;
public class VM_DoctorTimeSlotsPerDay : ListingLogFields
{
    public int DayId { get; set; }
    public string DoctorId { get; set; }
    public List<DoctorTimeSlots> DoctorSlots { get; set; } = new List<DoctorTimeSlots>();
}
public class DoctorTimeSlots
{
    public string DoctorTime { get; set; }
}

