
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Patient.Quries.GetDoctorSlots;
public class GetDoctorTimeSlotsByDayIdAndDoctorId : IRequest<IResponse>
{
    public int DayId { get; set; }
    public string DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
}


