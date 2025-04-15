namespace PatientDoctor.Application.Helpers.SMSRequest;
    public class ScheduleSmsRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public DateTime SendAt { get; set; }
    }
