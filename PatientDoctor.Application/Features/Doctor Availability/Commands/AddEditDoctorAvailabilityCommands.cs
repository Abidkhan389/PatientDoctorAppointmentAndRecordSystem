using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Doctor_Availability.Commands;
public class AddEditDoctorAvailabilityCommands : IRequest<IResponse>
{
    public  Guid? Id { get; set; }
    public string DoctorId { get; set; }
    public int AppointmentDurationMinutes { get; set; }
    public List<int> DayIds { get; set; }

    public List<DoctorTimeSlot> DoctorTimeSlots { get; set; } = new List<DoctorTimeSlot>();
}

public class DoctorTimeSlot
{
    public string StartTime { get; set; }  // Example: "08:00 AM"
    public string EndTime { get; set; }    // Example: "11:00 PM"
}


