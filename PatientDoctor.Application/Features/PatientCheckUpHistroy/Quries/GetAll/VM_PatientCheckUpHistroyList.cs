
using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetAll;
   public class VM_PatientCheckUpHistroyList : ListingLogFields
{
    public Guid PrescriptionId { get; set; }
    public Guid PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DoctorId { get; set; }
    public string DoctorName { get; set; }
    public string patientCnic { get; set; }
    public string patientCity { get; set; }
    public string PatientPhoneNumber { get; set; }
    public string Plan { get; set; }
    public int Status { get; set; }
}

