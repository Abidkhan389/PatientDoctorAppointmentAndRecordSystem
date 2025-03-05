

using PatientDoctor.Application.Features.Doctor_Availability.Commands;
using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
public class VM_DoctorAvailabilites : ListingLogFields
{
    public Guid AvailabilityId { get; set; }
    public string DoctorId { get; set; }
    public string DoctorName { get; set; }
    public int DayId { get; set; }
    public string DayName { get; set; }
    public List<DoctorTimeSlot> DoctorTimeSlots { get; set; } = new List<DoctorTimeSlot>();
    public int AppointmentDurationMinutes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int Status { get; set; }
}

