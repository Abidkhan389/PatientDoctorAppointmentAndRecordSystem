
namespace PatientDoctor.Application.Features.Patient.Quries
{
    public class GetPatientList 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        // public DateTime? PatientAppoitmentTime { get; set; }
        //public int? Status { get; set; }

        public string? UserId { get; set; }
        public string? Cnic { get; set; }
        public string? MobileNumber { get; set; }
        public string? City { get; set; }
        public DateTime? appoitmentDate { get; set; }   
    }
}
