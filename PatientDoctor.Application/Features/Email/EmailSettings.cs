namespace PatientDoctor.Application.Features.Email;
public class EmailSettings
{
    public string SMTP { get; set; }
    public int SMTPPort { get; set; }
    public bool EnableSSL { get; set; }
    public string SenderEmail { get; set; }
    public string AppPassword { get; set; }
    public string EmailPassword { get; set; }
}


