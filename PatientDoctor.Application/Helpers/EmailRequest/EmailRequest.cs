namespace PatientDoctor.Application.Helpers.EmailRequest;
public class EmailRequest
{
    public string ToEmail { get; set; }
    public string? FromEmail { get; set; }
    public string Subject { get; set; }
    public string BodyContent { get; set; }
}
