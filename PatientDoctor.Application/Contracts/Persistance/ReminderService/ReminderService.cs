using PatientDoctor.Application.Contracts.Persistance.IEmail;
using PatientDoctor.Application.Contracts.Persistance.IReminderServices;
using PatientDoctor.Application.Helpers.EmailRequest;

namespace PatientDoctor.Application.Contracts.Persistance.ReminderService;
public class ReminderService : IReminderService
{
    private readonly IEmailRepository _emailRepository; // Assuming you have an email service
    //private readonly ISmsService _smsService; // Assuming you have an SMS service

    public ReminderService(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository;
        //_smsService = smsService;
    }
    public void SendReminder(int appointmentId,string FromEmail, string ToEmail, string phoneNumber, string message)
    {
       
        //SendReminderEmailAsync(email, message);
        //SendReminderSmsAsync(phoneNumber, message);

    }
    public async Task SendReminderEmailAsync(string DoctorId,EmailRequest model)
    {
        await _emailRepository.SendSchedulerEmailAsync(DoctorId,model); // Send the email using your email service
    }
    public async Task SendReminderSmsAsync(Guid AppointmentId,string reminderMessage)
    {
         //await _smsService.SendSmsAsync(phoneNumber, message); // Send SMS using your SMS service
    }
}
