namespace PatientDoctor.Application.Features.Patient.Quries.GetPatientDetailForPdf
{
    #nullable disable
    public class VM_GetPatientDetailForPdf
    {
        public string PatientName {  get; set; }
        public string City { get; set; }
        public string PatientMobileNumber {  get; set; }
        public string PatientCnic {  get; set; }
        public DateTime PatientCheckUpDate {  get; set; }
        public int PatientCheckUpDoctorFee { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
    }
}
