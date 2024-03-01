
namespace PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription.PatientCheckedUpFeeHistroy
{
    public class PatientCheckedUpFeeHistroyDto
    {
        public PatientCheckedUpFeeHistroyDto()
        {
            
        }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
        public string? DoctorNumber { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public string? PatientNumber { get; set; }
        public string PatientCnic { get; set; }
        public int CheckUpFee { get; set; }
    }
}
