using PatientDoctor.Application.Helpers.AppointmentSms;
namespace PatientDoctor.Application.Contracts.Persistance.ISmsRepository;
public interface IPatientAppointmentSmsRepository
{
    Task<bool> SendSmsAsync(PatientAppointmentSmsRequest Model);
}

