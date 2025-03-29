using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidaysByDoctorIdForPatientAppointment;
public    class GetDoctorHolidayByDoctorIdForPatientAppointment : IRequest<IResponse>
{
    public string DoctorId { get; set; }
}

