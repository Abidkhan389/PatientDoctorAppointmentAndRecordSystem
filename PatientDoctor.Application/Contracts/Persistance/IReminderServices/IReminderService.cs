using PatientDoctor.Application.Helpers.EmailRequest;

namespace PatientDoctor.Application.Contracts.Persistance.IReminderServices;
public interface IReminderService
{
   Task SendReminderEmailAsync(string DoctorId, EmailRequest model);
   Task SendReminderSmsAsync(Guid AppointmentId, string reminderMessage);
}
