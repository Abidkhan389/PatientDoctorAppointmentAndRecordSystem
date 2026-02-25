
namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidaysByDoctorIdForPatientAppointment;
public class GetDoctorHolidayByDoctorIdForPatientAppointmentHandler(IDoctorHolidayRepository _doctorHolidayRepository) : IRequestHandler<GetDoctorHolidayByDoctorIdForPatientAppointment, IResponse>
{
    public async Task<IResponse> Handle(GetDoctorHolidayByDoctorIdForPatientAppointment request, CancellationToken cancellationToken)
    {
        return await _doctorHolidayRepository.GetDoctorHolidayByDoctorIdForPatientAppointment(request);
    }
}

