
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.EmailRequest;

namespace PatientDoctor.Application.Contracts.Persistance.IEmail;
public   interface IEmailRepository
{
    Task<IResponse> SendEmailAsync(EmailRequest emailRequest);
    Task<IResponse> SendSchedulerEmailAsync(string DoctorUserId,EmailRequest emailRequest);
}

