using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PatientDoctor.Application.Contracts.Persistance.IEmail;
using PatientDoctor.Application.Features.Email;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.EmailRequest;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using System.Net;
using System.Net.Mail;

namespace PatientDoctor.Infrastructure.Repositories.Email;
public class EmailRepository : IEmailRepository
{
    private readonly EmailSettings _emailSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DocterPatiendDbContext _context;

    public EmailRepository(IOptions<EmailSettings> emailSettings,
        UserManager<ApplicationUser> userManager, DocterPatiendDbContext context)
    {
        _emailSettings = emailSettings.Value; // Correct way to access properties
        _userManager = userManager;
        _context = context;
    }

    public async Task<IResponse> SendEmailAsync(EmailRequest emailRequest)
    {
        try
        {
            using (var client = new SmtpClient(_emailSettings.SMTP, _emailSettings.SMTPPort))
            {
                client.EnableSsl = _emailSettings.EnableSSL; // Using EnableSSL from settings
                client.UseDefaultCredentials = false;

                // Using the AppPassword for authentication instead of the Gmail password
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.AppPassword);

                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                string emailBody = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 80%;
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        border-radius: 8px;
                        box-shadow: 0px 4px 8px rgba(0,0,0,0.1);
                        overflow: hidden;
                    }}
                    .header {{
                        background: #007bff;
                        color: #ffffff;
                        text-align: center;
                        padding: 20px;
                        font-size: 24px;
                        font-weight: bold;
                    }}
                    .content {{
                        padding: 20px;
                        color: #333333;
                        line-height: 1.6;
                    }}
                    .footer {{
                        background: #007bff;
                        color: white;
                        text-align: center;
                        padding: 15px;
                        font-size: 14px;
                    }}
                    .btn {{
                        display: inline-block;
                        background: #007bff;
                        color: #ffffff;
                        padding: 10px 20px;
                        text-decoration: none;
                        border-radius: 5px;
                        margin-top: 20px;
                    }}
                    .btn:hover {{
                        background: #0056b3;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>Eye Care Clinic</div>
                    <div class='content'>
                        <p>Dear User,</p>
                        <p>{emailRequest.BodyContent}</p>
                        <p>We appreciate your time and look forward to serving you!</p>
                        <a href='https://yourwebsite.com' class='btn'>Visit Our Website</a>
                    </div>
                    <div class='footer'>
                        &copy; {DateTime.Now.Year} Eye Care Clinic. All rights reserved.
                    </div>
                </div>
            </body>
            </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail),
                    Subject = emailRequest.Subject,
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailRequest.ToEmail);

                await client.SendMailAsync(mailMessage);
            }

            return new Response { Success = true, Message = "Email sent successfully." };
        }
        catch (Exception ex)
        {
            return new Response { Success = false, Message = $"Email sending failed: {ex.Message}" };
        }
    }
    public async Task<IResponse> SendSchedulerEmailAsync(string DoctorUserId, EmailRequest emailRequest)
    {
        try
        {
            var doctorObj = await _userManager.FindByIdAsync(DoctorUserId);
            if (doctorObj is not null)
            {

                using (var client = new SmtpClient(_emailSettings.SMTP, _emailSettings.SMTPPort))
            {
                client.EnableSsl = _emailSettings.EnableSSL; // Using EnableSSL from settings
                client.UseDefaultCredentials = false;

                // Using the AppPassword for authentication instead of the Gmail password
                client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.AppPassword);

                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                string emailBody = $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 80%;
                        max-width: 600px;
                        margin: 20px auto;
                        background: #ffffff;
                        border-radius: 8px;
                        box-shadow: 0px 4px 8px rgba(0,0,0,0.1);
                        overflow: hidden;
                    }}
                    .header {{
                        background: #007bff;
                        color: #ffffff;
                        text-align: center;
                        padding: 20px;
                        font-size: 24px;
                        font-weight: bold;
                    }}
                    .content {{
                        padding: 20px;
                        color: #333333;
                        line-height: 1.6;
                    }}
                    .footer {{
                        background: #007bff;
                        color: white;
                        text-align: center;
                        padding: 15px;
                        font-size: 14px;
                    }}
                    .btn {{
                        display: inline-block;
                        background: #007bff;
                        color: #ffffff;
                        padding: 10px 20px;
                        text-decoration: none;
                        border-radius: 5px;
                        margin-top: 20px;
                    }}
                    .btn:hover {{
                        background: #0056b3;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>Eye Care Clinic</div>
                    <div class='content'>
                        <p>Dear User,</p>
                        <p>{emailRequest.BodyContent}</p>
                        <p>We appreciate your time and look forward to serving you!</p>
                        <a href='https://yourwebsite.com' class='btn'>Visit Our Website</a>
                    </div>
                    <div class='footer'>
                        &copy; {DateTime.Now.Year} Eye Care Clinic. All rights reserved.
                    </div>
                </div>
            </body>
            </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail),
                    Subject = emailRequest.Subject,
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailRequest.ToEmail);

                await client.SendMailAsync(mailMessage);
                }
                var userdetails = await _context.Userdetail.Where(x => x.UserId == doctorObj.Id).FirstOrDefaultAsync();
                if (userdetails != null)
                {
                    userdetails.IsNotified = true;
                    _context.Userdetail.Update(userdetails);
                    await _context.SaveChangesAsync();
                }
                return new Response { Success = true, Message = "Email sent successfully." };
            }
            else
            {
                return new Response { Success = false, Message = $"Email Scheduler sending failed, canot find User with UserId : {DoctorUserId}" };
            }
        }
        catch (Exception ex)
        {
            return new Response { Success = false, Message = $"Email sending failed: {ex.Message}" };
        }
    }
}

