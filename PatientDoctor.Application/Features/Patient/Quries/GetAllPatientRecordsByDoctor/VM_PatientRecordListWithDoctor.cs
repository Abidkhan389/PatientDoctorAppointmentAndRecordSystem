using PatientDoctor.Application.Helpers.General;
namespace PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor
{
    #nullable enable
    public class VM_PatientRecordListWithDoctor : ListingLogFields
    {
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientCnic { get; set; }
        public DateTime? PatientCheckUpDate { get; set; }
        public int PatientCheckUpDoctorFee { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmail { get; set; }
        public string DoctorId { get; set; }
    }
}
