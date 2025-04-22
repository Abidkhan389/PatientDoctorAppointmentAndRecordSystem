namespace PatientDoctor.Application.Helpers.AppointmentSms;
public class PatientAppointmentSmsRequest
{
    public string PatientMobileNumber { get; set; }    // Patient's mobile number
    public string DoctorName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string TimeSlot { get; set; }
   
}

